using LotusAscend.AppDataContext;
using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using LotusAscend.Models;

namespace LotusAscend.Services;

/// <summary>
/// Handles the business logic for redeeming loyalty points for coupons.
/// </summary>
public class CouponService : ICouponService
{
    private readonly AppDbContext _context;

    public CouponService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Redeems a member's points for a discount coupon if the amount is valid and the member has enough points.
    /// </summary>
    /// <param name="memberId">The ID of the member redeeming the points.</param>
    /// <param name="request">The request containing the number of points to redeem.</param>
    /// <returns>A ServiceResult containing details of the redeemed coupon and the member's remaining points.</returns>
    public async Task<ServiceResult<CouponResponse>> RedeemCouponAsync(string memberId, RedeemCouponRequest request)
    {
        if (!int.TryParse(memberId, out var id))
        {
            return new ServiceResult<CouponResponse>(null, false, "Invalid member ID.");
        }

        var member = await _context.Members.FindAsync(id);
        if (member == null)
        {
            return new ServiceResult<CouponResponse>(null, false, "Member not found.");
        }

        // Expanded logic for specific coupon values
        decimal couponValue = request.PointsToRedeem switch
        {
            250 => 25,
            500 => 50,
            1000 => 100,
            1500 => 150,
            2500 => 250,
            5000 => 500,
            10000 => 1000,
            _ => 0 // Default case for invalid amounts
        };

        if (couponValue == 0)
        {
            return new ServiceResult<CouponResponse>(null, false, "Invalid points amount. Please redeem one of the allowed values.");
        }

        if (member.TotalPoints < request.PointsToRedeem)
        {
            return new ServiceResult<CouponResponse>(null, false, "Not enough points.");
        }

        // Subtract points from the member's account and record the coupon
        member.TotalPoints -= request.PointsToRedeem;
        var coupon = new Coupon
        {
            MemberId = member.Id,
            PointsRedeemed = request.PointsToRedeem,
            CouponValue = couponValue,
            CouponCode = GenerateCouponCode() // <-- Generate and assign the code
        };

        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        
        var response = new CouponResponse(
            $"Successfully redeemed a ₹{couponValue} coupon!",
            coupon.CouponCode,                                 
            member.TotalPoints
        );

        return new ServiceResult<CouponResponse>(response, true, null);
    }
    
    /// <summary>
    /// Generates a simple, random 8-character alphanumeric coupon code.
    /// </summary>
    /// <returns>A unique coupon code string.</returns>
    private string GenerateCouponCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
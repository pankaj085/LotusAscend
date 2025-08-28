using LotusAscend.Contracts;

namespace LotusAscend.Interfaces;

/// <summary>
/// Defines the contract for the coupon redemption service.
/// </summary>
public interface ICouponService
{
    /// <summary>
    /// Redeems a member's loyalty points for a discount coupon.
    /// </summary>
    /// <param name="memberId">The unique identifier for the member.</param>
    /// <param name="request">The details of the redemption request.</param>
    /// <returns>A response containing the details of the redeemed coupon.</returns>
    Task<ServiceResult<CouponResponse>> RedeemCouponAsync(string memberId, RedeemCouponRequest request);
}
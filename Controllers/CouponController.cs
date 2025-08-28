using System.Security.Claims;
using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotusAscend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // This endpoint requires authentication
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    
    /// <summary>
    /// Redeems a specific amount of points for a discount coupon.
    /// </summary>
    /// <remarks>
    /// Valid point amounts for redemption are 250, 500, 1000, 1500, 2500, 5000, and 10000.
    /// </remarks>
    /// <param name="request">The request containing the number of points to redeem.</param>
    /// <returns>A confirmation of the successful redemption, including the coupon value.</returns>
    /// <response code="200">Returns a success message, coupon value, and remaining points.</response>
    /// <response code="400">If the points amount is invalid or the member has insufficient points.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost("redeem")]
    [ProducesResponseType(typeof(CouponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RedeemCoupon(RedeemCouponRequest request)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (memberId == null) return Unauthorized();
        
        var result = await _couponService.RedeemCouponAsync(memberId, request);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }
}
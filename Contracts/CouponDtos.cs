using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts;

// Data needed to redeem points for a coupon.
public record RedeemCouponRequest([Required][Range(1, int.MaxValue)] int PointsToRedeem);

// The response after a successful coupon redemption.
public record CouponResponse(string Message, int PointsRedeemed, decimal CouponValue, int RemainingPoints);
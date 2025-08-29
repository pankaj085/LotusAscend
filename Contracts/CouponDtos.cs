using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts;

// Represents the data required to redeem points for a coupon.
public record RedeemCouponRequest([Required][Range(1, int.MaxValue)] int PointsToRedeem);

// Represents the response sent after a successful coupon redemption.
public record CouponResponse(string Message, string NewCouponCode, int RemainingPoints);


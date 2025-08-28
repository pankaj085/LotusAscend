namespace LotusAscend.Models;

/// <summary>
/// Represents a discount coupon that a member has redeemed using points.
/// </summary>
public class Coupon
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int PointsRedeemed { get; set; }
    public decimal CouponValue { get; set; }
    public DateTime RedemptionDate { get; set; } = DateTime.UtcNow;

    public Member? Member { get; set; }
}
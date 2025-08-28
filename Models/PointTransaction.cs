namespace LotusAscend.Models;

/// <summary>
/// Represents a single transaction where a member earned points.
/// </summary>
public class PointTransaction
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public decimal PurchaseAmount { get; set; }
    public int PointsAdded { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public Member? Member { get; set; }
}
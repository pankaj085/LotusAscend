using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Models;

/// <summary>
/// Represents a member of the loyalty program.
/// </summary>
public class Member
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    [StringLength(15)]
    public required string MobileNumber { get; set; }
    public bool IsVerified { get; set; } = false;
    public int TotalPoints { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OTP? Otp { get; set; }
    public ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();
    public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}
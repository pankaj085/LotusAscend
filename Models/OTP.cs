using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Models;

/// <summary>
/// Represents a One-Time Password used for member verification.
/// </summary>
public class OTP
{
    public int Id { get; set; }
    public int MemberId { get; set; }

    [Required]
    [StringLength(6)]
    public required string Code { get; set; }
    public DateTime Expiry { get; set; }

    public Member? Member { get; set; }
}
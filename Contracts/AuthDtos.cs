using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts;

// Data needed to register a new member.
public record RegisterRequest(
    [Required]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")] string MobileNumber,
    [Required] string Username);

// Data needed to verify a member's OTP.
public record VerifyRequest(
    [Required]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
    string MobileNumber, 
    
    [Required]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "OTP must be exactly 4 digits.")]
    [RegularExpression("^[0-9]{4}$", ErrorMessage = "OTP must contain only digits.")]
    string Otp);

// The response containing the JWT after successful login.
public record AuthResponse(string Token);
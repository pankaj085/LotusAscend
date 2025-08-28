using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts;

// Data needed to register a new member.
public record RegisterRequest([Required] string MobileNumber, [Required] string Username);

// Data needed to verify a member's OTP.
public record VerifyRequest([Required] string MobileNumber, [Required] string Otp);

// The response containing the JWT after successful login.
public record AuthResponse(string Token);
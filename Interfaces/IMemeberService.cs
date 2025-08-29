using LotusAscend.Contracts;

namespace LotusAscend.Interfaces;


/// <summary>
/// Defines the contract for member registration and verification services.
/// </summary>
public interface IMemberService
{

    Task<ServiceResult<string>> LoginAsync(LoginRequest request);
    
    /// <summary>
    /// Registers a new member.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<ServiceResult<string>> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Verifies a member's account using an OTP.
    /// </summary>
    /// <param name="request">The verification details.</param>
    /// <returns>An authentication response with a JWT token upon success.</returns>
    Task<ServiceResult<AuthResponse>> VerifyAsync(VerifyRequest request);
}

public record ServiceResult<T>(T? Data, bool IsSuccess, string? ErrorMessage);
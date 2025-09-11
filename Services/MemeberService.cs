using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LotusAscend.AppDataContext;
using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using LotusAscend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LotusAscend.Services;

/// <summary>
/// Handles the business logic for member registration and authentication.
/// </summary>
public class MemberService : IMemberService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public MemberService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new member if the username and mobile number are not already in use.
    /// </summary>
    /// <param name="request">The registration request containing the member's details.</param>
    /// <returns>A service result indicating the outcome of the registration attempt.</returns>
    public async Task<ServiceResult<string>> RegisterAsync(RegisterRequest request)
    {

        // Check for existing mobile number
        var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.MobileNumber == request.MobileNumber);
        if (existingMember != null)
        {
            return new ServiceResult<string>(null, false, "Mobile number already registered.");
        }

        // Check for existing username
        if (await _context.Members.AnyAsync(m => m.Username == request.Username))
        {
            return new ServiceResult<string>(null, false, "Username is already taken.");
        }

        var member = new Member
        {
            MobileNumber = request.MobileNumber,
            Username = request.Username
        };

        var num = new Random();
        string randomotp = num.Next(1000, 9999).ToString();

        // For simplicity, using a fixed dummy OTP.
        var otp = new OTP  
        {
            Member = member,
            Code = randomotp, // date we got this assignment - 27th August(08)  "2708"
            Expiry = DateTime.UtcNow.AddMinutes(2)
        };

        _context.Members.Add(member);
        _context.Otps.Add(otp);
        await _context.SaveChangesAsync();

        Console.WriteLine($"OTP for {request.MobileNumber}: {randomotp}");

        return new ServiceResult<string>("Registration successful. Please verify with OTP.", true, null);
    }

    /// <summary>
    /// Initiates the login process for an existing member by sending an OTP.
    /// </summary>
    public async Task<ServiceResult<string>> LoginAsync(LoginRequest request)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.MobileNumber == request.MobileNumber);

        if (member == null)
        {
            return new ServiceResult<string>(null, false, "This mobile number is not registered.");
        }

        // Remove any old OTP for this user to ensure a clean login attempt
        var existingOtp = await _context.Otps.FirstOrDefaultAsync(o => o.MemberId == member.Id);
        if (existingOtp != null)
        {
            _context.Otps.Remove(existingOtp);
        }

        var num = new Random();
        string randomotp = num.Next(1000, 9999).ToString();

        // Generate a new OTP for the existing user
        var otp = new OTP
        {
            MemberId = member.Id,
            Code = randomotp, // Your dummy OTP
            Expiry = DateTime.UtcNow.AddMinutes(5)
        };

        _context.Otps.Add(otp);
        await _context.SaveChangesAsync();

        Console.WriteLine($"OTP for {request.MobileNumber}: {randomotp}");

        return new ServiceResult<string>("OTP sent successfully. Please verify to log in.", true, null);
    }

    /// <summary>
    /// Verifies a member's account with the provided OTP and issues a JWT token upon success.
    /// </summary>
    /// <param name="request">The verification request containing the mobile number and OTP.</param>
    /// <returns>A service result containing an authentication response with a JWT token.</returns>
    public async Task<ServiceResult<AuthResponse>> VerifyAsync(VerifyRequest request)
    {
        var member = await _context.Members.Include(m => m.Otp)
            .FirstOrDefaultAsync(m => m.MobileNumber == request.MobileNumber);

        if (member == null || member.Otp == null || member.Otp.Code != request.Otp)
        {
            return new ServiceResult<AuthResponse>(null, false, "Invalid mobile number or OTP.");
        }

        if (member.Otp.Expiry < DateTime.UtcNow)
        {
            return new ServiceResult<AuthResponse>(null, false, "OTP has expired.");
        }

        member.IsVerified = true;
        _context.Otps.Remove(member.Otp);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(member);
        return new ServiceResult<AuthResponse>(new AuthResponse(token), true, null);
    }

    /// <summary>
    /// Generates a JWT token for an authenticated member.
    /// </summary>
    /// <param name="member">The member for whom to generate the token.</param>
    /// <returns>A JWT token string.</returns>
    private string GenerateJwtToken(Member member)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, member.Username),
            new Claim("mobileNumber", member.MobileNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
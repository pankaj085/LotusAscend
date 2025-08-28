using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LotusAscend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    /// <summary>
    /// Registers a new member with a unique username and mobile number.
    /// </summary>
    /// <param name="request">The registration details containing the username and mobile number.</param>
    /// <returns>A confirmation message upon successful registration.</returns>
    /// <response code="200">Returns a success message.</response>
    /// <response code="400">If the mobile number or username is already registered.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _memberService.RegisterAsync(request);
        return result.IsSuccess ? Ok(new { message = result.Data }) : BadRequest(new { error = result.ErrorMessage });
    }

    /// <summary>
    /// Verifies a member's registration using the provided OTP.
    /// </summary>
    /// <param name="request">The verification details containing the mobile number and OTP.</param>
    /// <returns>A JWT token upon successful verification.</returns>
    /// <response code="200">Returns the JWT authentication token.</response>
    /// <response code="401">If the mobile number or OTP is incorrect.</response>
    [HttpPost("verify")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Verify(VerifyRequest request)
    {
        var result = await _memberService.VerifyAsync(request);
        return result.IsSuccess ? Ok(result.Data) : Unauthorized(new { error = result.ErrorMessage });
    }
}
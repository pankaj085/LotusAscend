using System.Security.Claims;
using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotusAscend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // This endpoint requires authentication
public class PointsController : ControllerBase
{
    private readonly IPointsService _pointsService;

    public PointsController(IPointsService pointsService)
    {
        _pointsService = pointsService;
    }

    /// <summary>
    /// Adds loyalty points to the authenticated member's account.
    /// </summary>
    /// <remarks>
    /// Points are calculated based on the rule: 10 points for every ₹100 of purchase amount.
    /// </remarks>
    /// <param name="request">The request containing the total purchase amount.</param>
    /// <returns>The member's new total points balance.</returns>
    /// <response code="200">Returns the updated total points.</response>
    /// <response code="400">If the purchase amount is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost("add")]
    [ProducesResponseType(typeof(PointsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPoints(AddPointsRequest request)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (memberId == null) return Unauthorized();

        var result = await _pointsService.AddPointsAsync(memberId, request);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    /// <summary>
    /// Retrieves the total loyalty points for the authenticated member.
    /// </summary>
    /// <returns>The member's current total points.</returns>
    /// <response code="200">Returns the current total points.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the member is not found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PointsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPoints()
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (memberId == null) return Unauthorized();

        var result = await _pointsService.GetPointsAsync(memberId);
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { error = result.ErrorMessage });
    }
}
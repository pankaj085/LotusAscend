using LotusAscend.AppDataContext;
using LotusAscend.Contracts;
using LotusAscend.Interfaces;
using LotusAscend.Models;

namespace LotusAscend.Services;

/// <summary>
/// Handles the business logic for managing member loyalty points.
/// </summary>
public class PointsService : IPointsService
{
    private readonly AppDbContext _context;

    public PointsService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Calculates and adds loyalty points to a member's account based on a purchase.
    /// </summary>
    /// <param name="memberId">The ID of the member earning the points.</param>
    /// <param name="request">The request containing the purchase amount.</param>
    /// <returns>A ServiceResult containing the member's new total points balance.</returns>
    public async Task<ServiceResult<PointsResponse>> AddPointsAsync(string memberId, AddPointsRequest request)
    {
        if (!int.TryParse(memberId, out var id))
        {
            return new ServiceResult<PointsResponse>(null, false, "Invalid member ID.");
        }

        var member = await _context.Members.FindAsync(id);
        if (member == null)
        {
            return new ServiceResult<PointsResponse>(null, false, "Member not found.");
        }

        int pointsToAdd = (int)(request.PurchaseAmount / 100) * 10;

        if (pointsToAdd > 0)
        {
            member.TotalPoints += pointsToAdd;
            var transaction = new PointTransaction
            {
                MemberId = member.Id,
                PurchaseAmount = request.PurchaseAmount,
                PointsAdded = pointsToAdd
            };
            _context.PointTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        return new ServiceResult<PointsResponse>(new PointsResponse(member.TotalPoints), true, null);
    }

    /// <summary>
    /// Retrieves the current total points for a given member.
    /// </summary>
    /// <param name="memberId">The ID of the member whose points are being requested.</param>
    /// <returns>A ServiceResult containing the member's current total points.</returns>
    public async Task<ServiceResult<PointsResponse>> GetPointsAsync(string memberId)
    {
        if (!int.TryParse(memberId, out var id))
        {
            return new ServiceResult<PointsResponse>(null, false, "Invalid member ID.");
        }

        var member = await _context.Members.FindAsync(id);
        if (member == null)
        {
            return new ServiceResult<PointsResponse>(null, false, "Member not found.");
        }

        return new ServiceResult<PointsResponse>(new PointsResponse(member.TotalPoints), true, null);
    }
}
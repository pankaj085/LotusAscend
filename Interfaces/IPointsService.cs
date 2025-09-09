using LotusAscend.Contracts;

namespace LotusAscend.Interfaces;

/// <summary>
/// Defines the contract for managing member loyalty points.
/// </summary>
public interface IPointsService
{
    /// <summary>
    /// Adds loyalty points to a member's account based on a purchase amount.
    /// </summary>
    /// <param name="memberId">The unique identifier for the member.</param>
    /// <param name="request">The details of the purchase transaction.</param>
    /// <returns>The updated points information for the member.</returns>
    Task<ServiceResult<PointsResponse>> AddPointsAsync(string memberId, AddPointsRequest request);

    /// <summary>
    /// Retrieves the current points balance for a member.
    /// </summary>
    /// <param name="memberId">The unique identifier for the member.</param>
    /// <returns>The current points information for the member.</returns>
    Task<ServiceResult<PointsResponse>> GetPointsAsync(string memberId);

    Task<ServiceResult<PagedResult<TransactionHistoryResponse>>> GetTransactionHistoryAsync(string memberId, int pageNumber, int pageSize);
}
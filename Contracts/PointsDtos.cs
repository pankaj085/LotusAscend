using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts;

// Data needed to add points from a purchase.
public record AddPointsRequest([Required][Range(1, double.MaxValue)] decimal PurchaseAmount);

// A simple response showing a member's total points.
public record PointsResponse(int TotalPoints);

// A general response with key details about a member.
public record MemberResponse(int Id, string Username, string MobileNumber, int TotalPoints, bool IsVerified);
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Application.DTOs.Output;

public record TripResponse(
    int Id,
    string Title,
    string Description,
    TripStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<RouteResponse> Routes,

    // Why is he ???
    UserResponse? User = null // you need to remove or think about where to be used
);
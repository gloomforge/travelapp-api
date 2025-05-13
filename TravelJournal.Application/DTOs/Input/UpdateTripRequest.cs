namespace TravelJournal.Application.DTOs.Input;

public record UpdateTripRequest(
    string? Title = null,
    string? Description = null,
    string? TripStatus = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    DateTime? UpdatedAt = null
)
{
    public string TripStatus { get; init; } = TripStatus ?? Domain.ValueObjects.TripStatus.None.Value;
    public DateTime? StartDate { get; init; } = StartDate ?? DateTime.UtcNow;
    public DateTime? EndDate { get; init; } = EndDate ?? DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; } = UpdatedAt ?? DateTime.UtcNow;
}
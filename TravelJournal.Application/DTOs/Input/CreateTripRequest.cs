namespace TravelJournal.Application.DTOs.Input;

public record CreateTripRequest(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate
);
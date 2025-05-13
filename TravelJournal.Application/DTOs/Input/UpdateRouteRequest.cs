namespace TravelJournal.Application.DTOs.Input;

public record UpdateRouteRequest(
    string? LocationName = null,
    string? Country = null,
    string? City = null
);
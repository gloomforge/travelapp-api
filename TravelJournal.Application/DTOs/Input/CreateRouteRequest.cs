namespace TravelJournal.Application.DTOs.Input;

public record CreateRouteRequest(
    string LocationName,
    string Country,
    string City
);
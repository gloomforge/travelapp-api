namespace TravelJournal.Application.DTOs.Output;

public record RouteResponse(
    int Id,
    string LocationName,
    string Country,
    string City
);
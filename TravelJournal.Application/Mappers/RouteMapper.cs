using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Domain.Models;

namespace TravelJournal.Application.Mappers;

public class RouteMapper
{
    public static Route ToModel(CreateRouteRequest request)
    {
        return new Route(
            request.LocationName,
            request.Country,
            request.City
        );
    }

    public static RouteResponse ToResponse(Route route)
    {
        return new RouteResponse(
            route.Id,
            route.LocationName,
            route.Country,
            route.City
        );
    }
}
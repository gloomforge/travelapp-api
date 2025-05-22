using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Domain.Models;

namespace TravelJournal.Application.Mappers;

public static class RouteMapper
{
    public static Route ToModel(CreateRouteRequest request)
    {
        var route = new Route(
            request.LocationName,
            request.Country,
            request.City
        )
        {
            TripId = request.TripId
        };

        return route;
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
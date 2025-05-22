using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Domain.Models;

namespace TravelJournal.Application.Mappers;

public static class TripMapper
{
    public static Trip ToModel(CreateTripRequest request)
    {
        return new Trip(
            request.Title,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.UserId
        );
    }
    
    public static TripResponse ToResponse(Trip trip)
    {
        return new TripResponse(
            trip.Id,
            trip.Title,
            trip.Description,
            trip.Status,
            trip.StartDate,
            trip.EndDate,
            trip.CreatedAt,
            trip.UpdatedAt,
            trip.Routes.Select(RouteMapper.ToResponse).ToList()
        );
    }
}
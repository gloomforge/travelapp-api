using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;

namespace TravelJournal.Application.Interfaces;

public interface IRouteService
{
    // TODO: Basic method for working with route
    Task<RouteResponse> CreateRouteAsync(CreateRouteRequest request);
    Task<RouteResponse> UpdateRouteAsync(int id, UpdateRouteRequest request);
    Task<bool> DeleteRouteAsync(int id);
    Task<RouteResponse?> FindRouteByIdAsync(int id);
    Task<List<RouteResponse>> FindRoutesByLocationAsync(string location);
    Task<List<RouteResponse>> FindRoutesByCountryAsync(string country);
    Task<List<RouteResponse>> FindRoutesByCityAsync(string city);
}
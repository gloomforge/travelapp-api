using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;
using TravelJournal.Application.Mappers;
using TravelJournal.Domain.Interfaces;

namespace TravelJournal.Application.Services;

public class RouteService(
    IRouteRepository repo
) : IRouteService
{
    public async Task<RouteResponse> CreateRouteAsync(CreateRouteRequest request)
    {
        var route = RouteMapper.ToModel(request);

        await repo.AddRoute(route);
        return RouteMapper.ToResponse(route);
    }

    public async Task<RouteResponse> UpdateRouteAsync(int id, UpdateRouteRequest request)
    {
        var existing = await repo.FindById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Route with id {id} was not found");

        if (!string.IsNullOrWhiteSpace(request.LocationName))
            existing.LocationName = request.LocationName;
        if (!string.IsNullOrWhiteSpace(request.Country))
            existing.Country = request.Country;
        if (!string.IsNullOrWhiteSpace(request.City))
            existing.City = request.City;
        
        await repo.UpdateRoute(existing);
        return RouteMapper.ToResponse(existing);
    }

    public async Task<bool> DeleteRouteAsync(int id)
    {
        var existing = await repo.FindById(id);
        if (existing == null)
            return false;
        
        await repo.DeleteRoute(existing);
        return true;
    }

    public async Task<RouteResponse?> FindRouteByIdAsync(int id)
    {
        var route = await repo.FindById(id);
        if (route == null)
            throw new KeyNotFoundException($"Route with id {id} was not found");
        return RouteMapper.ToResponse(route);
    }

    public async Task<List<RouteResponse>> FindRoutesByLocationAsync(string location)
    {
        var routes = await repo.FindRoutesByLocation(location);
        return routes.Select(RouteMapper.ToResponse).ToList();
    }

    public async Task<List<RouteResponse>> FindRoutesByCountryAsync(string country)
    {
        var routes = await repo.FindRoutesByCountry(country);
        return routes.Select(RouteMapper.ToResponse).ToList();
    }

    public async Task<List<RouteResponse>> FindRoutesByCityAsync(string city)
    {
        var routes = await repo.FindRoutesByCity(city);
        return routes.Select(RouteMapper.ToResponse).ToList();
    }
}
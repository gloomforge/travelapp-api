using TravelJournal.Domain.Models;

namespace TravelJournal.Domain.Interfaces;

public interface IRouteRepository
{
    Task<Route?> FindById(int id);
    Task<List<Route>> FindRoutesByLocation(string location);
    Task<List<Route>> FindRoutesByCountry(string country);
    Task<List<Route>> FindRoutesByCity(string city);
    Task AddRoute(Route route);
    Task UpdateRoute(Route route);
    Task DeleteRoute(Route route);
}
using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.Models;
using TravelJournal.Infrastructure.Data;

namespace TravelJournal.Infrastructure.Repositories;

public class RouteRepository(RouteDbContext context) : IRouteRepository
{
    public async Task AddRoute(Route route)
    {
        context.Routes.Add(route);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRoute(Route route)
    {
        context.Routes.Update(route);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRoute(Route route)
    {
        context.Routes.Remove(route);
        await context.SaveChangesAsync();
    }

    public Task<Route?> FindById(int id) =>
        context.Routes.SingleOrDefaultAsync(t => t.Id == id);

    public Task<List<Route>> FindRoutesByLocation(string location) =>
        context.Routes.Where(t => t.LocationName == location).ToListAsync();

    public Task<List<Route>> FindRoutesByCountry(string country) =>
        context.Routes.Where(t => t.Country == country).ToListAsync();

    public Task<List<Route>> FindRoutesByCity(string city) =>
        context.Routes.Where(t => t.City == city).ToListAsync();
}
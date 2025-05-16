using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.Models;
using TravelJournal.Infrastructure.Data;

namespace TravelJournal.Infrastructure.Repositories;

// public class TripRepository(TripDbContext context) : ITripRepository
public class TripRepository(ApplicationDbContext context) : ITripRepository
{
    public async Task AddTrip(Trip trip)
    {
        context.Trips.Add(trip);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateTrip(Trip trip)
    {
        context.Trips.Update(trip);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTrip(Trip trip)
    {
        context.Trips.Remove(trip);
        await context.SaveChangesAsync();
    }

    public Task<List<Trip>> FindTripsByTitle(string title) =>
        context.Trips.Where(t => t.Title == title).ToListAsync();

    public Task<Trip?> FindById(int id) =>
        context.Trips.SingleOrDefaultAsync(t => t.Id == id);

    public Task<bool> ExistsById(int id) =>
        context.Trips.AnyAsync(t => t.Id == id);
}
using TravelJournal.Domain.Models;

namespace TravelJournal.Domain.Interfaces;

public interface ITripRepository
{
    Task<Trip?> FindById(int id);
    Task<List<Trip>> FindTripsByTitle(string title);
    Task AddTrip(Trip trip);
    Task UpdateTrip(Trip trip);
    Task DeleteTrip(Trip trip);
    Task<bool> ExistsById(int id);
}
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;

namespace TravelJournal.Application.Interfaces;

public interface ITripService
{
    // TODO: Basic methods for working with travel
    Task<TripResponse> CreateTripAsync(CreateTripRequest request);
    Task<TripResponse> UpdateTripAsync(int id, UpdateTripRequest request);
    Task<bool> DeleteTripAsync(int id);
    Task<List<TripResponse>> FindAllTripsAsync();
    Task<TripResponse> FindTripByIdAsync(int id);
    Task<List<TripResponse>> FindTripsByTitleAsync(string title);
    Task<List<TripResponse>> FindTripsByUserIdAsync(int userId);
}
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;

namespace TravelJournal.Application.Interfaces;

public interface ITripService
{
    // TODO: Basic methods for working with travel
    Task<TripResponse> CreateTaskAsync(CreateTripRequest request);
    Task<TripResponse> UpdateTaskAsync(int id, UpdateTripRequest request);
    Task<bool> DeleteTaskAsync(int id);
    Task<List<TripResponse>> FindAllTasksAsync();
    Task<TripResponse> FindTaskByIdAsync(int id);
    Task<List<TripResponse>> FindTasksByTitleAsync(string title);
}
using TravelJournal.Application.DTOs.Output;

namespace TravelJournal.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> GetUserByIdAsync(int id);
    Task<List<TripResponse>> GetUserTripsAsync(int userId);
} 
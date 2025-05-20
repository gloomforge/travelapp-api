using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;
using TravelJournal.Application.Mappers;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Application.Services;

public class TripService(
    ITripRepository repo,
    IUserRepository userRepo
) : ITripService
{
    public async Task<TripResponse> CreateTripAsync(CreateTripRequest request)
    {
        var userExists = await userRepo.ExistsById(request.UserId);
        if (!userExists)
            throw new KeyNotFoundException($"User with id {request.UserId} not found");
            
        var trip = TripMapper.ToModel(request);

        await repo.AddTrip(trip);
        return TripMapper.ToResponse(trip);
    }

    public async Task<TripResponse> UpdateTripAsync(int id, UpdateTripRequest request)
    {
        var existing = await repo.FindById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Trip with id {id} not found");

        if (!string.IsNullOrWhiteSpace(request.Title))
            existing.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description))
            existing.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.TripStatus))
            existing.Status = TripStatus.From(request.TripStatus);
        if (request.StartDate.HasValue)
            existing.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue)
            existing.EndDate = request.EndDate.Value;
        existing.UpdatedAt = request.UpdatedAt ?? DateTime.UtcNow;

        await repo.UpdateTrip(existing);
        return TripMapper.ToResponse(existing);
    }

    public async Task<bool> DeleteTripAsync(int id)
    {
        var existing = await repo.FindById(id);
        if (existing == null)
            return false;

        await repo.DeleteTrip(existing);
        return true;
    }

    public async Task<List<TripResponse>> FindAllTripsAsync()
    {
        var trips = await repo.FindTripsByTitle(string.Empty);
        return trips.Select(TripMapper.ToResponse).ToList();
    }

    public async Task<TripResponse> FindTripByIdAsync(int id)
    {
        var trip = await repo.FindById(id);
        if (trip == null)
            throw new KeyNotFoundException($"Trip with id {id} not found");
            
        var tripResponse = TripMapper.ToResponse(trip);
        
        var user = await userRepo.FindUserById(trip.UserId);
        if (user != null)
        {
            tripResponse = tripResponse with { User = UserMapper.ToResponse(user) };
        }
        
        return tripResponse;
    }

    public async Task<List<TripResponse>> FindTripsByTitleAsync(string title)
    {
        var trips = await repo.FindTripsByTitle(title);
        return trips.Select(TripMapper.ToResponse).ToList();
    }
    
    public async Task<List<TripResponse>> FindTripsByUserIdAsync(int userId)
    {
        var userExists = await userRepo.ExistsById(userId);
        if (!userExists)
            throw new KeyNotFoundException($"User with id {userId} not found");
            
        var trips = await repo.FindTripsByUserId(userId);
        var responses = trips.Select(TripMapper.ToResponse).ToList();
        
        var user = await userRepo.FindUserById(userId);
        if (user == null) return responses;
        var userResponse = UserMapper.ToResponse(user);
        responses = responses.Select(r => r with { User = userResponse }).ToList();

        return responses;
    }
}
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;
using TravelJournal.Application.Mappers;
using TravelJournal.Domain.Interfaces;

namespace TravelJournal.Application.Services;

public class UserService(IUserRepository userRepo, ITripRepository tripRepo) : IUserService
{
    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        var user = await userRepo.FindUserById(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id {id} not found");
            
        return UserMapper.ToResponse(user);
    }
    
    public async Task<List<TripResponse>> GetUserTripsAsync(int userId)
    {
        var userExists = await userRepo.ExistsById(userId);
        if (!userExists)
            throw new KeyNotFoundException($"User with id {userId} not found");
            
        var trips = await tripRepo.FindTripsByUserId(userId);
        var tripResponses = trips.Select(TripMapper.ToResponse).ToList();
        
        var user = await userRepo.FindUserById(userId);
        if (user != null)
        {
            var userResponse = UserMapper.ToResponse(user);
            tripResponses = tripResponses.Select(r => r with { User = userResponse }).ToList();
        }
        
        return tripResponses;
    }
} 
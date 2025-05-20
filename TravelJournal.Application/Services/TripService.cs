using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;
using TravelJournal.Application.Mappers;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Application.Services;

public class TripService(
    ITripRepository repo
) : ITripService
{
    public async Task<TripResponse> CreateTripAsync(CreateTripRequest request)
    {
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
        return TripMapper.ToResponse(trip);
    }

    public async Task<List<TripResponse>> FindTripsByTitleAsync(string title)
    {
        var trips = await repo.FindTripsByTitle(title);
        return trips.Select(TripMapper.ToResponse).ToList();
    }
}
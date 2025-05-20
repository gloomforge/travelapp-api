using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Domain.Models;

public class Trip
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TripStatus Status { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public List<Route> Routes { get; private set; } = [];
    // public User User { get; set; } = null!;

    public Trip(string title, string description, DateTime startDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = TripStatus.None;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRoute(Route route)
    {
        ArgumentNullException.ThrowIfNull(route);
        route.SetTrip(this);
        Routes.Add(route);
    }
}
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Domain.Models;

public class Trip
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TripStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    private Trip()
    {
    }

    public Trip(string title, string description, DateTime startDate, DateTime endDate, int userId)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        UserId = userId;
        Status = TripStatus.None;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
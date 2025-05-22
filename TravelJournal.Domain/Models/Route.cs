namespace TravelJournal.Domain.Models;

public class Route
{
    public int Id { get; init; }
    public string LocationName { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    
    public int TripId { get; set; }
    public Trip Trip { get; set; }

    private Route()
    {
    }

    public Route(string locationName, string country, string city)
    {
        LocationName = locationName;
        Country = country;
        City = city;
    }

    internal void SetTrip(Trip trip)
    {
        Trip = trip ?? throw new ArgumentNullException(nameof(trip));
        TripId = trip.Id;
    }
}
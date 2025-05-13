namespace TravelJournal.Domain.Models;

public class Route
{
    public int Id { get; set; }
    public string LocationName { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    private Route()
    {
    }

    public Route(string locationName, string country, string city)
    {
        LocationName = locationName;
        Country = country;
        City = city;
    }
}
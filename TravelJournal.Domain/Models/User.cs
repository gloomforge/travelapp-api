using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Email Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    private User()
    {
    }

    public User(string name, Email email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
    }
}
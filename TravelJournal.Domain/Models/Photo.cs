namespace TravelJournal.Domain.Models;

// make either avatars to the user or for a trip card (in question)
public class Photo
{
    public int Id { get; set; }
    public string FilePath { get; set; } = null!;
}
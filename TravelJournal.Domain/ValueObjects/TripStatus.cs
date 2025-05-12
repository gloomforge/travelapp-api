namespace TravelJournal.Domain.ValueObjects;

public class TripStatus : IEquatable<TripStatus>
{
    public static readonly TripStatus Planned = new("Planned");
    public static readonly TripStatus Active = new("Active");
    public static readonly TripStatus Completed = new("Completed");
    public static readonly TripStatus None = new("None");

    public string Value { get; private set; }

    private TripStatus(string value)
    {
        Value = value;
    }

    public static TripStatus From(string value)
    {
        return value switch
        {
            "Planned" => Planned,
            "Active" => Active,
            "Completed" => Completed,
            _ => throw new ArgumentException($"Invalid trip status: {value}")
        };
    }

    public override string ToString() => Value;

    public bool Equals(TripStatus? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is TripStatus other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
}
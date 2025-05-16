using System.Text.RegularExpressions;

namespace TravelJournal.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email(string value)
    {
#pragma warning disable SYSLIB1045
        if (!Regex.IsMatch(value, @"^\S+@\S+\.\S+$"))
            throw new ArgumentException("Invalid email format");
#pragma warning restore SYSLIB1045

        Value = value;
    }

    public static Email Create(string value) => new Email(value);
    public override bool Equals(object? obj) => obj is Email email && Value == email.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
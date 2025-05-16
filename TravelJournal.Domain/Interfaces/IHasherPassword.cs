namespace TravelJournal.Domain.Interfaces;

public interface IHasherPassword
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
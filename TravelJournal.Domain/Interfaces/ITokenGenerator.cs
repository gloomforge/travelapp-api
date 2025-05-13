namespace TravelJournal.Domain.Interfaces;

public interface ITokenGenerator
{
    string GenerateToken(int id, string username);
}
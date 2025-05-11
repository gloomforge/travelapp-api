using TravelJournal.Domain.Interfaces;

namespace TravelJournal.Infrastructure.Security;

public class BCryptPassword : IHasherPassword
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hashPassword) =>
        BCrypt.Net.BCrypt.Verify(password, hashPassword);
}
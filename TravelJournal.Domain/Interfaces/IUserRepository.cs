using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> FindUserById(int id);
    Task<User?> FindUserByEmail(Email email);
    Task<User?> FindUserByName(string username);
    Task AddUser(User user);
    Task<bool> ExistsByName(string name);
    Task<bool> ExistsByEmail(Email email);
    Task<bool> ExistsById(int id);
}
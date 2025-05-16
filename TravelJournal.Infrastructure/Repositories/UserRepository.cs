using Microsoft.EntityFrameworkCore;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;
using TravelJournal.Infrastructure.Data;

namespace TravelJournal.Infrastructure.Repositories;

// public class UserRepository(UserDbContext context) : IUserRepository
public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task AddUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsByName(string name) => 
        context.Users.AnyAsync(u => u.Name == name);

    public Task<bool> ExistsByEmail(Email email) => 
        context.Users.AnyAsync(u => u.Email == email);

    public Task<User?> FindUserByEmail(Email email) =>
        context.Users.SingleOrDefaultAsync(u => u.Email == email);
    
    public Task<User?> FindUserByName(string username) =>
        context.Users.SingleOrDefaultAsync(u => u.Name == username);
}
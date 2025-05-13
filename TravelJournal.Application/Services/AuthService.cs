using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Application.Services;

public class AuthService(
    IUserRepository repo,
    IHasherPassword hasher,
    ITokenGenerator get)
    : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await repo.ExistsByName(request.Name))
            throw new ArgumentException("User with this name already exists");

        if (await repo.ExistsByEmail(Email.Create(request.Email)))
            throw new ArgumentException("User with this email already exists");

        var hashPassword = hasher.Hash(request.Password);
        var user = new User(request.Name, Email.Create(request.Email), hashPassword);

        await repo.AddUser(user);
        
        var token = get.GenerateToken(user.Id, user.Name);
        return new AuthResponse(
            token,
            new UserResponse(
                user.Id,
                user.Name,
                user.Email.Value,
                user.CreatedAt
            )
        );
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        User? user;
        try
        {
            var emailVo = Email.Create(request.Login);
            user = await repo.FindUserByEmail(emailVo);
        }
        catch (ArgumentException)
        {
            user = await repo.FindUserByName(request.Login);
        }

        if (user is null || !hasher.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = get.GenerateToken(user.Id, user.Name);
        return new AuthResponse(
            token,
            new UserResponse(
                user.Id,
                user.Name,
                user.Email.Value,
                user.CreatedAt
            )
        );
    }
}
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;

namespace TravelJournal.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
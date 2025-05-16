namespace TravelJournal.Application.DTOs.Output;

public record AuthResponse(
    string Token,
    UserResponse User
);
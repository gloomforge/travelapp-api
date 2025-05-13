namespace TravelJournal.Application.DTOs.Input;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
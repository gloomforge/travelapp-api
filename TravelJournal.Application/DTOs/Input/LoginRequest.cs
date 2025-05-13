namespace TravelJournal.Application.DTOs.Input;

public record LoginRequest(
    string Login,
    string Password
);
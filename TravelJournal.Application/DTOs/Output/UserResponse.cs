namespace TravelJournal.Application.DTOs.Output;

public record UserResponse(
    int Id,
    string Name,
    string Email,
    DateTime CreatedAt
);
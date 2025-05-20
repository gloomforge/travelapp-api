using TravelJournal.Application.DTOs.Output;
using TravelJournal.Domain.Models;

namespace TravelJournal.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email.Value,
            user.CreatedAt
        );
    }
} 
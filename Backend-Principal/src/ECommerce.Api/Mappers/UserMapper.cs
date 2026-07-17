using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user) =>
        new(user.Id, user.Email, user.Name, user.Role);
}

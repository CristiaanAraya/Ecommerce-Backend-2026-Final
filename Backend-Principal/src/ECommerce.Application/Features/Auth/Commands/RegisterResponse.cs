namespace ECommerce.Application.Features.Auth.Commands;

public record RegisterResponse(Guid Id, string Email, string Name, string Role);

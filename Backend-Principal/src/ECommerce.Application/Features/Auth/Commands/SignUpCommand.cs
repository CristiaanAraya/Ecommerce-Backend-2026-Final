using MediatR;

namespace ECommerce.Application.Features.Auth.Commands;

public record SignUpCommand(string Email, string Name, string Password) : IRequest<RegisterResponse>;

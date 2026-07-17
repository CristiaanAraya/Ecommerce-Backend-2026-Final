using MediatR;

namespace ECommerce.Application.Features.Auth.Commands;

public record SignInCommand(string Email, string Password) : IRequest<string?>;

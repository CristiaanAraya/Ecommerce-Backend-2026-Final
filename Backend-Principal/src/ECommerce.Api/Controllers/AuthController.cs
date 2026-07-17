using ECommerce.Api.DTOs;
using ECommerce.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] SignUpRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new SignUpCommand(request.Email, request.Name, request.Password), ct);
        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] SignInRequest request, CancellationToken ct)
    {
        var token = await mediator.Send(new SignInCommand(request.Email, request.Password), ct);

        if (token is null)
            return Unauthorized(new { message = "Email o contraseña incorrectos." });

        return Ok(new TokenResponse(token));
    }
}

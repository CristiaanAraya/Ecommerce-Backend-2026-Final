namespace ECommerce.Api.DTOs;

public record SignUpRequest(string Email, string Name, string Password);
public record SignInRequest(string Email, string Password);
public record TokenResponse(string AccessToken);
public record UserResponse(Guid Id, string Email, string Name, string Role);

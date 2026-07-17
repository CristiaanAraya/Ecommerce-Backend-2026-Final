using ECommerce.Domain.Entities;

namespace ECommerce.Application.Contracts.Infrastructure;

public interface ITokenService
{
    string GenerateToken(User user);
}

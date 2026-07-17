using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;

namespace ECommerce.Infrastructure.Services;

public class PasswordHasher : IHashService
{
    public string ComputeHash(string plainText)
        => BCrypt.Net.BCrypt.HashPassword(plainText);

    public bool CheckHash(string plainText, string hash)
        => BCrypt.Net.BCrypt.Verify(plainText, hash);
}

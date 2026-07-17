namespace ECommerce.Application.Contracts.Infrastructure;

public interface IHashService
{
    string ComputeHash(string plainText);
    bool CheckHash(string plainText, string hash);
}

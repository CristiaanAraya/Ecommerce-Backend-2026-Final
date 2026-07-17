using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "Customer";
    public DateTime CreatedAt { get; private set; }

    public Address? ShippingAddress { get; private set; }

    private User() { }

    public User(string email, string name, string passwordHash, string role = "Customer")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new BusinessException("La contraseña hasheada es obligatoria.");
        if (role is not "Customer" and not "Admin")
            throw new BusinessException("El rol debe ser Customer o Admin.");

        Id = Guid.NewGuid();
        Email = new Email(email);
        Name = name.Trim();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignRole(string role)
    {
        if (role is not "Customer" and not "Admin")
            throw new BusinessException("El rol debe ser Customer o Admin.");
        Role = role;
    }

    public void SetShippingAddress(string street, string city, string? state = null, string? zipCode = null, string? country = null)
    {
        ShippingAddress = new Address(street, city, state, zipCode, country);
    }
}

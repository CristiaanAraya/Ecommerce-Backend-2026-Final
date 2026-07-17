using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public Category(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("El nombre de la categoría es obligatorio.");

        Id = id;
        Name = name.Trim();
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new BusinessException("El nuevo nombre no puede estar vacío.");
        Name = newName.Trim();
    }
}

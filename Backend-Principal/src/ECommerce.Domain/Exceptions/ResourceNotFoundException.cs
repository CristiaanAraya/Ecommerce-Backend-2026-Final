namespace ECommerce.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string resource, object id)
        : base($"{resource} con id '{id}' no fue encontrado.") { }
}

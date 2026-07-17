namespace Stock.Domain.Exceptions;

public class BusinessException : DomainException
{
    public BusinessException(string message) : base(message) { }
}

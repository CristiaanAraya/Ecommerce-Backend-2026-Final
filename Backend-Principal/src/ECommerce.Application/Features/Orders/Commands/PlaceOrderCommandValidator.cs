using FluentValidation;

namespace ECommerce.Application.Features.Orders.Commands;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El usuario es requerido.");

        RuleFor(x => x.Lines)
            .NotEmpty().WithMessage("La orden debe contener al menos un producto.");

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).NotEmpty().WithMessage("El ID del producto es requerido.");
            line.RuleFor(l => l.Quantity).InclusiveBetween(1, 100).WithMessage("La cantidad debe estar entre 1 y 100.");
        });
    }
}

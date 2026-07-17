using FluentValidation;

namespace Stock.Application.Features.Stock.Commands;

public class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
{
    public ReserveStockCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La lista de items no puede estar vacía.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("El ID del producto es requerido.");

            item.RuleFor(i => i.Quantity)
                .InclusiveBetween(1, 10000).WithMessage("La cantidad debe estar entre 1 y 10000.");
        });
    }
}

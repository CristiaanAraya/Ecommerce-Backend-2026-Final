using FluentValidation;

namespace Stock.Application.Features.Stock.Commands;

public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("El ID del producto es requerido.");

        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 100000).WithMessage("La cantidad debe estar entre 1 y 100000.");
    }
}

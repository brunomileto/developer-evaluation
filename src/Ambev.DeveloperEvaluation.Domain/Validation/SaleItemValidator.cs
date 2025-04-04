using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for individual SaleItem in a Sale
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(i => i.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(i => i.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100);

        RuleFor(i => i.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(i => i.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");

        RuleFor(i => i.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount must be zero or positive.");

        RuleFor(i => i.Total)
            .GreaterThanOrEqualTo(0).WithMessage("Total must be zero or positive.");
    }
}
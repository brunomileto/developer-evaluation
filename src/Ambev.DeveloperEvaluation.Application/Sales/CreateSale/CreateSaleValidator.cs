using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for creating a sale.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId and BranchId: Must not be empty
    /// - CustomerName and BranchName: Required and limited in length
    /// - Items: Must contain at least one item and each item must follow rules
    /// - Item.Quantity: Must be between 1 and 20
    /// - Item.ProductName: Required
    /// - Item.UnitPrice: Must be greater than 0
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty().MaximumLength(100);

        RuleFor(sale => sale.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(sale => sale.BranchName)
            .NotEmpty().MaximumLength(100);

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(sale => sale.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            item.RuleFor(i => i.ProductName)
                .NotEmpty().MaximumLength(100);

            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0);
        });
    }
}

using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId and BranchId: Must not be empty
    /// - CustomerName and BranchName: Required and length limited
    /// - Items: Must contain at least one valid item
    /// </remarks>
    public CreateSaleRequestValidator()
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
            .NotEmpty().WithMessage("At least one item is required.");

        RuleForEach(sale => sale.Items).SetValidator(new CreateSaleItemRequestValidator());
    }
}
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale entity, ensuring all business rules and data integrity are respected.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        var now = DateTime.UtcNow;
        
        RuleFor(s => s.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(30);

        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(s => s.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100);

        RuleFor(s => s.Status)
            .NotEqual(Status.Cancelled)
            .WithMessage("Sale must be active.");

        RuleFor(s => s.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(s => s.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100);

        RuleFor(s => s.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(now).WithMessage("Sale date cannot be in the future.");

        RuleFor(s => s.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Total amount must be zero or positive.");

        RuleFor(s => s.Items)
            .NotEmpty().WithMessage("At least one item is required in the sale.");

        RuleFor(x => x)
            .Must(sale => new ActiveSaleSpecification().IsSatisfiedBy(sale))
            .WithMessage("Sale must be active");
        
        RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());

    }
}
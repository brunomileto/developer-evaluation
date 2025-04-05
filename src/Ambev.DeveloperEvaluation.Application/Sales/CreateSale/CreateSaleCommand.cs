using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command captures the required data to create a sale, 
/// including customer and branch info, and itemized product details. 
/// It implements <see cref="IRequest{TResponse}"/> to return a <see cref="CreateSaleResult"/>.
/// Validation is handled by <see cref="CreateSaleCommandValidator"/>.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the external customer ID.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name at the time of sale (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external branch ID.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date. Defaults to current UTC time.
    /// </summary>
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Unique number that identifies the sale (e.g., invoice number).
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of sale items to include in the sale.
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = [];

    /// <summary>
    /// Validates the command data using <see cref="CreateSaleCommandValidator"/>.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

using Ambev.DeveloperEvaluation.Common.Extensions;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale, including product details, quantity, and price.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// External identity: Product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Denormalized: Product name at time of sale.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Type of discount applied to this item based on quantity purchased.
    /// This represents the discount category (e.g., None, TenPercent, TwentyPercent), not a monetary value.
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.None;

    /// <summary>
    /// Type of discount applied to this item based on quantity purchased.
    /// This represents the discount category (e.g., None, TenPercent, TwentyPercent), not a monetary value.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Total amount for this item (after discount).
    /// </summary>
    public decimal Total { get; private set; }

    /// <summary>
    /// Gets the sale's item current status.
    /// Indicates whether the sale item is active, or cancelled in the system.
    /// </summary>
    public Status Status { get; set; } = Status.Active;

    /// <summary>
    /// Calculates discount and total according to business rules.
    /// </summary>
    public void CalculateTotal()
    {
        Discount = Quantity switch
        {
            > 20 => throw new InvalidOperationException("Cannot sell more than 20 units of a single product."),
            >= 10 => UnitPrice * Quantity * DiscountType.TwentyPercent.ToPercentage(),
            >= 4 => UnitPrice * Quantity * DiscountType.TenPercent.ToPercentage(),
            _ => DiscountType.None.ToPercentage()
        };

        Total = (UnitPrice * Quantity) - Discount;
    }
    
    /// <summary>
    /// Checks whether the current sale item has a valid quantity based on business rules.
    /// </summary>
    /// <returns>
    /// True if the quantity is greater than 0 and less than or equal to 20; otherwise, false.
    /// </returns>
    public bool IsValidQuantity()
    {
        var spec = new ValidQuantitySpecification();
        return spec.IsSatisfiedBy(this);
    }


    /// <summary>
    /// Validates the item using business rules.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }

    /// <summary>
    /// External identity: Sale ID to which this item belongs.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Navigation property: The sale this item belongs to.
    /// </summary>
    public Sale Sale { get; set; } = null!;
}
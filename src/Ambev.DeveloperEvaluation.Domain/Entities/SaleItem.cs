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
        Discount = UnitPrice * Quantity * DiscountType.ToPercentage();
        Total = (UnitPrice * Quantity) - Discount;
    }

    /// <summary>
    /// Determines the applicable discount type based on the quantity purchased.
    /// </summary>
    /// <param name="quantity">The quantity of the product in the sale item.</param>
    /// <returns>The appropriate <see cref="DiscountType"/> based on business rules.</returns>
    private static DiscountType GetDiscountType(int quantity)
    {

        if (quantity is <= 0 or > 20)
            return DiscountType.None;

        return quantity switch
        {
            >= 10 => DiscountType.TwentyPercent,
            >= 4 => DiscountType.TenPercent,
            _ => DiscountType.None
        };
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
    
    /// <summary>
    /// Creates a new instance of <see cref="SaleItem"/> using the provided details and automatically calculates
    /// discount and total based on quantity and unit price.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="unitPrice">The price per unit of the product.</param>
    /// <param name="quantity">The quantity of the product being sold.</param>
    /// <param name="saleId">The ID of the sale this item belongs to.</param>
    /// <returns>A fully initialized and calculated <see cref="SaleItem"/>.</returns>
    public static SaleItem Create(Guid productId, string productName, decimal unitPrice, int quantity, Guid saleId)
    {
        var item = new SaleItem
        {
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            DiscountType = GetDiscountType(quantity),
            SaleId = saleId,
            Status = Status.Active
        };

        item.CalculateTotal();
        return item;
    }
    
    /// <summary>
    /// Private constructor to enforce controlled creation via <see cref="Create"/> factory method.
    /// </summary>
    private SaleItem() { }
}
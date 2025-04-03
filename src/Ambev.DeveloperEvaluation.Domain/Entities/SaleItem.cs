using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

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
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item (after discount).
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets the sale's item current status.
    /// Indicates whether the sale item is active, or cancelled in the system.
    /// </summary>
    public Status Status { get; set; } = Status.Active;
}
using Ambev.DeveloperEvaluation.Domain.Common;

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
    /// Discount applied to this item (total discount, not %).
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item (after discount).
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Indicates whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}

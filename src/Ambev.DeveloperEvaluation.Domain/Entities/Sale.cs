using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system, including customer and itemized product information.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique number that identifies the sale (e.g., invoice number).
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// External identity: customer ID.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Denormalized: customer full name at the moment of the sale.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// External identity: branch ID where the sale occurred.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Denormalized: branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Total value of the sale (sum of all item totals).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Indicates whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// List of items that compose the sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        SaleDate = DateTime.UtcNow;
    }
}
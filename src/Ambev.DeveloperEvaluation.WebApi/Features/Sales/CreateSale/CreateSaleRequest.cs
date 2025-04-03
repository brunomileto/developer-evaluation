using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the ID of the customer placing the sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer's name at the time of sale (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the branch where the sale is made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch name at the time of sale (denormalized).
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date. If not provided, defaults to current server time.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the list of items being sold.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
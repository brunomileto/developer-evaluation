using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents a request to update an existing sale.
/// </summary>
public class UpdateSaleRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a product item in a sale response.
/// </summary>
public class CreateSaleItemResponse
{
    /// <summary>
    /// The name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total amount for this item (after discount).
    /// </summary>
    public decimal Total { get; set; }
}
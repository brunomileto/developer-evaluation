namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents a product item within a sale during the creation process.
/// </summary>
/// <remarks>
/// This DTO is used inside <see cref="CreateSaleCommand"/> to capture product-level details 
/// such as ID, name, unit price, and quantity for each item in the sale.
/// </remarks>
public class CreateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the product being sold.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product at the time of sale (denormalized).
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price per unit of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }
}
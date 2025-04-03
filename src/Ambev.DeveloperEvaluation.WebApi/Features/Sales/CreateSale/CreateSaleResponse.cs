namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateSale operation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// The unique identifier of the created sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The generated sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The sale date.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The name of the customer involved in the sale.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the branch where the sale occurred.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The total amount for the sale, including discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The list of product items included in the sale.
    /// </summary>
    public List<CreateSaleItemResponse> Items { get; set; } = new();
}
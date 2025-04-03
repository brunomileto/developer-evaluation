namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Represents a single sale record in a paginated result
/// </summary>
public class GetSalesListItemResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}
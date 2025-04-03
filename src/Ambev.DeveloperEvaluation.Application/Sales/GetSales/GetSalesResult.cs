namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result of a paginated sales query
/// </summary>
public class GetSalesResult
{
    public List<GetSalesListItemResult> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Response for a successful sale deletion
/// </summary>
public class DeleteSaleResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Sale deleted successfully.";
}
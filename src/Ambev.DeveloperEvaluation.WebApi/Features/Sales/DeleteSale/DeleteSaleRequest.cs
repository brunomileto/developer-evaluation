namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Request for deleting a sale (encapsulates the route ID if needed).
/// </summary>
public class DeleteSaleRequest
{
    public Guid Id { get; set; }
}
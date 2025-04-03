using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Query for retrieving a paginated list of sales with filters and ordering
/// </summary>
public class GetSalesQuery : IRequest<GetSalesResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public string? Status { get; set; }
}
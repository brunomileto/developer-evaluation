using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale
/// </summary>
public record UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    public Guid Id { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    
    public Status Status { get; init; } = Status.Active;
    public List<UpdateSaleItem> Items { get; init; } = [];
}
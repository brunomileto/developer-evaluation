using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command to delete a sale by ID
/// </summary>
public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResult>;
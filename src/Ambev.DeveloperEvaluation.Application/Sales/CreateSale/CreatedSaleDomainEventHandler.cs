using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;

public class CreatedSaleDomainEventHandler : INotificationHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<CreatedSaleDomainEventHandler> _logger;

    public CreatedSaleDomainEventHandler(ILogger<CreatedSaleDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event: SaleCreated | SaleId: {SaleId} | CustomerId: {customerId} | CreatedAt: {Time}", 
            notification.SaleId, notification.CustomerId, notification.CreatedAt);
        return Task.CompletedTask;
    }
}
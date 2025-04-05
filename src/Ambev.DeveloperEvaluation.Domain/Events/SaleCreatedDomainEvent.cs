using System;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Events
{
    public class SaleCreatedDomainEvent : IDomainEvent, INotification
    {
        public Guid SaleId { get; }
        public Guid CustomerId { get; }
        public DateTime CreatedAt { get; }

        public SaleCreatedDomainEvent(Guid saleId, Guid customerId, DateTime createdAt)
        {
            SaleId = saleId;
            CustomerId = customerId;
            CreatedAt = createdAt;
        }
    }
}
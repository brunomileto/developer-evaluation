using System;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Events
{
    public class SaleModifiedDomainEvent : IDomainEvent, INotification
    {
        public Guid SaleId { get; }
        public Guid CustomerId { get; }
        public DateTime ModifiedAt { get; }

        public SaleModifiedDomainEvent(Guid saleId, Guid customerId, DateTime modifiedAt)
        {
            SaleId = saleId;
            CustomerId = customerId;
            ModifiedAt = modifiedAt;
        }
    }
}
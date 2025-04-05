using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync(BaseEntity entity, CancellationToken cancellationToken)
        {
            var domainEvents = entity.DomainEvents.OfType<INotification>().ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            entity.ClearDomainEvents();
        }
    }
}
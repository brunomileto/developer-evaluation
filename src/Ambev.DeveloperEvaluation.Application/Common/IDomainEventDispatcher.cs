using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(BaseEntity entity, CancellationToken cancellationToken);
    }
}
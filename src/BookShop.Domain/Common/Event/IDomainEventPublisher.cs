
namespace BookShop.Domain.Common.Event
{
    public interface IDomainEventPublisher
    {

        public Task PublishAsync<DomainEvent>(DomainEvent @event) where DomainEvent : IDomainEvent;

    }


}

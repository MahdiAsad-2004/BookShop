using BookShop.Domain.Common.Event;
using BookShop.Domain.Exceptions;
using MediatR;

namespace BookShop.Application.Common.Event
{
    public class DomianEventPublisher : IDomainEventPublisher
    {
        #region Constructor

        private readonly IMediator _mediator;
        public DomianEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion


        public async Task PublishAsync<DomainEvent>(DomainEvent @event) where DomainEvent : IDomainEvent
        {
            if (@event.GetType().IsAssignableTo(typeof(INotification)) == false)
                throw new EventCantHandleException("");
            
            await _mediator.Publish(@event);
        }

    }
}

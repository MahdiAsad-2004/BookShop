using MediatR;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;

namespace BookShop.Application.Common.Event
{
    public class EntityUpdatedEvent<TEntity, TKey> : EntityUpdateedEventBase<TEntity, TKey> , INotification
        where TEntity : Entity<TKey>
    {
        public EntityUpdatedEvent(TEntity entity , DateTime eventDateTime , Guid userId) : base(entity , eventDateTime , userId)
        {   
        }
    }
}

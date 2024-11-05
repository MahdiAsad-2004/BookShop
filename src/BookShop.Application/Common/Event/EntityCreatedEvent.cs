using MediatR;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;

namespace BookShop.Application.Common.Event
{
    public class EntityCreatedEvent<TEntity, TKey> : EntityCreatedEventBase<TEntity, TKey> , INotification
        where TEntity : Entity<TKey>
    {
        public EntityCreatedEvent(TEntity entity , DateTime eventDateTime , Guid userId) : base(entity , eventDateTime , userId)
        {   
        }
    }
}

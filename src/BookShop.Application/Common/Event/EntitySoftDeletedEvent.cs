using MediatR;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;

namespace BookShop.Application.Common.Event
{
    public class EntitySoftDeletedEvent<TEntity, TKey> : EntitySoftDeletedEventBase<TEntity, TKey> , INotification
        where TEntity : Entity<TKey>
    {
        public EntitySoftDeletedEvent(TEntity entity , DateTime eventDateTime , Guid userId) : base(entity , eventDateTime , userId)
        {   
        }
    }
}

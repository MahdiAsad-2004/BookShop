using MediatR;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;

namespace BookShop.Application.Common.Event
{
    public class EntityDeletedEvent<TEntity, TKey> : EntityDeletedEventBase<TEntity, TKey> , INotification
        where TEntity : Entity<TKey>
    {
        public EntityDeletedEvent(TEntity entity , DateTime eventDateTime , Guid userId) : base(entity , eventDateTime , userId)
        {   
        }
    }
}

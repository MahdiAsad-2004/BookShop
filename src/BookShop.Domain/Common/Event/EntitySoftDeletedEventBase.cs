
namespace BookShop.Domain.Common.Event
{
    public abstract class EntitySoftDeletedEventBase<TEntity,TKey> : IDomainEvent
        where TEntity : Entity.Entity<TKey> 
    {

        public EntitySoftDeletedEventBase(TEntity entity , DateTime dateTime , Guid userId)
        {
            Entity = entity;
            EventDateTime = dateTime;
            UserId = userId;
        }

        
        public TEntity Entity { get; }
        public Guid UserId { get; init; }
        public DateTime EventDateTime { get; init;}





    }



}

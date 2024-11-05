

namespace BookShop.Domain.Common.Event
{
    public abstract class EntityCreatedEventBase<TEntity,TKey> : IDomainEvent
        where TEntity : Entity.Entity<TKey> 
    {

        public EntityCreatedEventBase(TEntity entity , DateTime dateTime , Guid userId)
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


namespace BookShop.Domain.Common.Event
{
    public abstract class EntityDeletedEventBase<TEntity,TKey> : IDomainEvent
        where TEntity : Entity.Entity<TKey> 
    {

        public EntityDeletedEventBase(TEntity entity , DateTime dateTime , Guid userId)
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

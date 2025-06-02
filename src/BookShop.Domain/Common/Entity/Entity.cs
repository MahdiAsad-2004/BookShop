using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;

namespace BookShop.Domain.Common.Entity
{
    public class Entity<TId> : IHasKey<TId>, IAuditable, ISoftDalete, IAggregateRoot
        //where TId : struct
    {

        public TId Id { get; set; } 
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public string CreateBy { get; set; } = string.Empty;
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime? DeleteDate { get; set; } = null;
        public string? DeletedBy { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public byte[] RowVersion { get; set; }


        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public async Task PublishAllDomainEvents(IDomainEventPublisher domainEventPublisher)
        {
            foreach (var domainEvent in _domainEvents)
            {
                await domainEventPublisher.PublishAsync(domainEvent);
            }
        }

        public async Task PublishAllDomainEventsAndClear(IDomainEventPublisher domainEventPublisher)
        {
            await PublishAllDomainEvents(domainEventPublisher);
            ClearDomainEvents();
        }


        public void SetPropertiesForCreate(TId id,string userId)
        {
            DateTime now = DateTime.UtcNow;
            Id = id;
            CreateBy = userId;
            CreateDate = now;
            DeleteDate = null;
            DeletedBy = null;
            LastModifiedBy = userId;
            LastModifiedDate = now;
            IsDeleted = false;
        }


    }
}

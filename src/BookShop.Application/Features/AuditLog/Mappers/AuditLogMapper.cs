using BookShop.Application.Common.Mapper.Attribute;
using BookShop.Application.Serializition;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Enums;

namespace BookShop.Application.Features.AuditLog.Mappers
{
    [InjectableMapper]
    public class AuditLogMapper
    {
        private readonly IJsonSerializer _jsonSerializer;
        public AuditLogMapper(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }



        public Domain.Entities.AuditLog ToAuditLog<TEntity, TKey, TDomainEvent>(TEntity entity, TDomainEvent domainEvent, AuditType auditType)
            where TEntity : Entity<TKey>
            where TDomainEvent : IDomainEvent
        {
            return new Domain.Entities.AuditLog
            {
                AuditType = auditType,
                Date = domainEvent.EventDateTime,
                EntityTypeFullName = entity.GetType().FullName,
                UserId = domainEvent.UserId,
                NewObject = _jsonSerializer.Serialize(entity),
                OldObject = _jsonSerializer.Serialize(entity),
            };
        }




    }



}

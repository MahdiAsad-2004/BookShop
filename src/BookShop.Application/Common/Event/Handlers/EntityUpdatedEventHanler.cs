using BookShop.Application.Features.AuditLog.Mappers;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Serilog;

namespace BookShop.Application.Common.Event.Handlers
{
    public class EntityUpdatedEventHandler<TEntity, TKey> : INotificationHandler<EntityUpdatedEvent<TEntity, TKey>>
        where TEntity : Entity<TKey>
    {
        #region constructor

        private readonly ILogger _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly AuditLogMapper _auditLogMapper;
        public EntityUpdatedEventHandler(ILogger logger, IAuditLogRepository auditLogRepository, AuditLogMapper auditLogMapper)
        {
            _logger = logger;
            _auditLogRepository = auditLogRepository;
            _auditLogMapper = auditLogMapper;
        }

        #endregion


        public async Task Handle(EntityUpdatedEvent<TEntity, TKey> notification, CancellationToken cancellationToken)
        {
            await _auditLogRepository.Add(_auditLogMapper.ToAuditLog<TEntity, TKey, EntityUpdatedEvent<TEntity, TKey>>
                (notification.Entity, notification, AuditType.Update));

            _logger.Information($"Domain Event: EntityUpdatedEvent<{typeof(TEntity).Name}> was handle.");
        }
    }
}

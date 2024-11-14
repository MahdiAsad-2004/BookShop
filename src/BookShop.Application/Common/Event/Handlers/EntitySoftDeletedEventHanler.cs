using BookShop.Application.Features.AuditLog.Mappers;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Serilog;

namespace BookShop.Application.Common.Event.Handlers
{
    public class EntitySoftDeletedEventHandler<TEntity, TKey> : INotificationHandler<EntitySoftDeletedEvent<TEntity, TKey>>
        where TEntity : Entity<TKey>
    {
        #region constructor

        private readonly ILogger _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly AuditLogMapper _auditLogMapper;
        public EntitySoftDeletedEventHandler(ILogger logger, IAuditLogRepository auditLogRepository, AuditLogMapper auditLogMapper)
        {
            _logger = logger;
            _auditLogRepository = auditLogRepository;
            _auditLogMapper = auditLogMapper;
        }

        #endregion


        public async Task Handle(EntitySoftDeletedEvent<TEntity, TKey> notification, CancellationToken cancellationToken)
        {
            await _auditLogRepository.Add(_auditLogMapper.ToAuditLog<TEntity, TKey, EntitySoftDeletedEvent<TEntity, TKey>>
                (notification.Entity, notification, AuditType.Update));

            _logger.Information($"Domain Event: EntitySoftDeletedEvent<{typeof(TEntity).Name}> handled.");
        }
    }
}

using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;
using MediatR;
using Serilog;
using BookShop.Application.Features.AuditLog.Mappers;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Common.Event.Handlers
{
    public class EntityDeletedEventHanler<TEntity, TKey> : INotificationHandler<EntityDeletedEvent<TEntity, TKey>>
        where TEntity : Entity<TKey>
    {
        #region constructor

        private readonly ILogger _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly AuditLogMapper _auditLogMapper;
        public EntityDeletedEventHanler(ILogger logger, IAuditLogRepository auditLogRepository, AuditLogMapper auditLogMapper)
        {
            _logger = logger;
            _auditLogRepository = auditLogRepository;
            _auditLogMapper = auditLogMapper;
        }

        #endregion



        public async Task Handle(EntityDeletedEvent<TEntity, TKey> notification, CancellationToken cancellationToken)
        {
            await _auditLogRepository.Add(_auditLogMapper.ToAuditLog<TEntity,TKey,EntityDeletedEvent<TEntity,TKey>>
                (notification.Entity ,notification ,AuditType.Delete));

            _logger.Information($"Domain Event: EntityDeletedEvent<{typeof(TEntity).Name}> was handle.");
        }
    }
}

using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IAuditLogRepository :
        IRepository,
        IReadRepository<AuditLog, Guid>,
        IWriteRepository<AuditLog, Guid>
    {
    }
}

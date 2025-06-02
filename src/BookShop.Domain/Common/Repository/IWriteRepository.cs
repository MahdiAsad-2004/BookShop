using BookShop.Domain.Common.Entity;
using System.Collections.ObjectModel;

namespace BookShop.Domain.Common.Repository
{
    public interface IWriteRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
        
    {
        Task Add(TEntity entity);

        Task<bool> Update(TEntity entity);

        Task<bool> SoftDelete(TEntity entity);

        Task<bool> SoftDelete(TKey key);





    }
}

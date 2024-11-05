using BookShop.Domain.Common.Entity;
using System.Collections.ObjectModel;

namespace BookShop.Domain.Common.Repository
{
    public interface IWriteRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
        
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task SoftDelete(TEntity entity);

        Task SoftDelete(TKey key);





    }
}

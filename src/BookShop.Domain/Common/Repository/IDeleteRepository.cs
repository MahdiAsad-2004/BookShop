using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Common.Repository
{
    public interface IDeleteRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task Delete(TEntity entity);

        Task Delete(TKey key);




    }
}

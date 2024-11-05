using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;

namespace BookShop.Domain.Common.Repository
{
    public interface ICrudRepository<TEntity, TKey> : 
        IReadRepository<TEntity,TKey> ,
        IWriteRepository<TEntity,TKey> ,
        IDeleteRepository<TEntity,TKey>
        where TEntity : Entity<TKey>
    {
    }


    public interface ICrudRepository<TEntity, TKey, TQueryOption> :
        IReadRepository<TEntity, TKey, TQueryOption>,
        IWriteRepository<TEntity, TKey>,
        IDeleteRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TQueryOption : IQueryOption<TEntity, TKey>, new()

    {
    }

}

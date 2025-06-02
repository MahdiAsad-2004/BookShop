using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using System.Collections.ObjectModel;

namespace BookShop.Domain.Common.Repository
{
    public interface IReadRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> Get(TKey key);

        Task<TEntity> Get(string key);

        Task<bool> IsExist(TKey key);


        //Task<IEnumerable<TEntity>> GetAll<TQueryOption>(Action<TQueryOption> configQueryOption)
        //    where TQueryOption : IQueryOption<TEntity, TKey>, new();

        //Task<TEntity> Get<TQueryOption>(TKey key, Action<TQueryOption> configQueryOption)
        //    where TQueryOption : IQueryOption<TEntity, TKey> , new();


    }

    public interface IReadRepository<TEntity, TKey, TQueryOption> : IReadRepository<TEntity , TKey>
       where TEntity : Entity<TKey>
       where TQueryOption : IQueryOption<TEntity, TKey>, new()
    {
        Task<IEnumerable<TEntity>> GetAll(TQueryOption? queryOption);

        Task<TEntity> Get(TKey key, TQueryOption? queryOption);

    }


}

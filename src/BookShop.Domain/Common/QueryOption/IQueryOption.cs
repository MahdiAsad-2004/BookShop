using BookShop.Domain.Common.Entity;
using BookShop.Domain.Entities;
namespace BookShop.Domain.Common.QueryOption
{
    public interface IQueryOption<TEntity,TKey> where TEntity : Entity<TKey>
    {
    }


   
}

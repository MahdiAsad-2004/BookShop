
using BookShop.Domain.Common.QueryOption;

namespace BookShop.Domain.Common.Entity
{
    public class PaginatedEntities<TEntity> where TEntity : Entity<Guid>
    {
        public List<TEntity> Entites { get; private set; }
        public Paging? Paging { get; private set; }
        public int TotalItemCount { get; private set; }


        public PaginatedEntities(List<TEntity> entities, Paging? paging, int totalItemCount)
        {
            Entites = entities;
            Paging = paging;
            TotalItemCount = totalItemCount;
        }

    }
}

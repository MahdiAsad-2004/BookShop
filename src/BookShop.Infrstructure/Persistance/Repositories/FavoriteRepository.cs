using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class FavoriteRepository : CrudRepository<Favorite, Guid> , IFavoriteRepository
    {
        public FavoriteRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task<bool> IsExist(Guid userId, Guid productId)
        {
            return await _dbSet.AnyAsync(a => a.IsDeleted == false && a.UserId == userId && a.ProductId == productId);  
        }


        public async Task<Favorite[]> GetAll(FavoriteQueryOption queryOption)
        {
            var query = _dbSet.AsNoTracking()
                .AsQueryable();

            //filters
            if (queryOption.UserId != null)
                query = query.Where(a => a.UserId == queryOption.UserId.Value);
        
            if (queryOption.ProductId != null)
                query = query.Where(a => a.UserId == queryOption.ProductId.Value);
        
            if (queryOption.FromCreateDate != null)
                query = query.Where(a => a.CreateDate >= queryOption.FromCreateDate.Value);
            
            if (queryOption.ToCreateDate != null)
                query = query.Where(a => a.CreateDate <= queryOption.ToCreateDate.Value);
            
            //sorting
            
            
            return query.ToArray();
        }


    }
}

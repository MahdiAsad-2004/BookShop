using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static BookShop.Domain.Constants.PermissionConstants;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class CartItemRepository : CrudRepository<CartItem, Guid>, ICartItemRepository
    {
        public CartItemRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task Create(CartItem cartItem)
        {
            int productStock = _dbContext.Set<Product>().Where(a => a.Id == cartItem.ProductId).Select(a => a.NumberOfInventory).FirstOrDefault();
            cartItem.Quantity = Math.Min(productStock, cartItem.Quantity);
            SetPropertiesForCreate(cartItem, Guid.NewGuid(), _currentUser.Id.ToString());
            await _dbSet.AddAsync(cartItem);
            await _dbContext.SaveChangesAsync();
            await cartItem.PublishAllDomainEvents(_domainEventPublisher);
        }


        public Task<bool> IsExist(Guid produtcId, Guid cartId)
        {
            return _dbSet.AnyAsync(a => a.ProductId == produtcId && a.CartId == cartId);
        }


        public async Task<Guid?> GetId(Guid produtcId, Guid cartId)
        {
            var Ids = await _dbSet.Where(a => a.ProductId == produtcId && a.CartId == cartId).Select(a => a.Id).ToListAsync();

            if (Ids.Any())
                return Ids[0];

            return null;
        }


        public async Task<bool> Update(Guid id, int quntity)
        {
            CartItem cartItem = await _dbSet.AsTracking().FirstAsync(a => a.Id == id);
            int productStock = _dbContext.Set<Product>().Where(a => a.Id == cartItem.ProductId).Select(a => a.NumberOfInventory).FirstOrDefault();
            //int newQuantity = cartItem.Quantity + quntity;
            //if (newQuantity > productStock)
            //{
            //    _dbContext.ChangeTracker.Clear();
            //    return false;
            //}
            //cartItem.Quantity = newQuantity;
            cartItem.Quantity = Math.Min(productStock , cartItem.Quantity + quntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task<List<CartItem>> GetAll(CartItemQueryOption queryOption, CartItemSortingOrder? sortingOrder = null)
        {
            var query = _dbSet.AsNoTracking()
                .AsQueryable()
                .Where(a => a.IsDeleted == false);
            
            Guid cartId = Guid.Empty;
            if (queryOption.UserId != null)
                cartId = await _dbContext.Carts.Where(a => a.UserId == queryOption.UserId.Value).Select(a => a.Id).FirstOrDefaultAsync();
            
            //includes
            if (queryOption.IncludeProduct)
                query = query.Include(a => a.Product);

            if (queryOption.IncludeDiscount)
                query = query.Include(a => a.Product)
                    .ThenInclude(a => a.Product_Discounts)
                    .ThenInclude(a => a.Discount);

            //filters
            if (queryOption.CreateDate != null)
                query = query.Where(a => a.CreateDate >= queryOption.CreateDate.Value);

            if (queryOption.ModifiedDate != null)
                query = query.Where(a => a.LastModifiedDate >= queryOption.ModifiedDate.Value);

            if (queryOption.MinQuantity != null)
                query = query.Where(a => a.Quantity >= queryOption.MinQuantity.Value);

            if (queryOption.MaxQuantity != null)
                query = query.Where(a => a.Quantity <= queryOption.MaxQuantity.Value);

            if (queryOption.UserId != null && cartId != Guid.Empty)
                query = query.Where(a => a.CartId == cartId);


            //sorting
            bool sorted = false;
            if (sortingOrder != null)
            {
                query = ApplyBaseSort(query, ref sorted, sortingOrder.Value);
            }

            return await query.ToListAsync();
        }


        public async Task<bool> SoftDelete(Guid id)
        {
            CartItem cartItem = new CartItem();
            string commandString = $@"
                UPDATE CartItems
                SET 
                [{nameof(cartItem.IsDeleted)}] = 1,
                [{nameof(cartItem.DeleteDate)}] = '{DateTime.UtcNow}',
                [{nameof(cartItem.DeletedBy)}] = '{_currentUser.Id}'
                WHERE [Id] = @id
                ";
            int rowAffected = 0;
            using (var conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await conn.OpenAsync();
                var command = new SqlCommand(commandString, conn);
                command.Parameters.AddWithValue("id", id);

                rowAffected = await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return rowAffected == 1;
        }




    }
}

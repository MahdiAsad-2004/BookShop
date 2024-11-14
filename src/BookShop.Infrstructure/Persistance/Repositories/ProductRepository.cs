using AutoMapper.QueryableExtensions;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class ProductRepository : CrudRepository<Product, Guid>, IProductRepository
    {
        public ProductRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        private Product MapToProduct(Product product, bool reviewsAccepted, bool includeDiscounts)
        {
            if (reviewsAccepted)
            {
                product.Reviews = product.Reviews.Where(a => a.IsAccepted);
            }
            if (includeDiscounts)
            {
                product.DiscountedPrice = product.Product_Discounts
                    .Select(a => a.Discount)
                    .OrderBy(a => a.Priority)
                    .FirstOrDefault(a => true)?.CalculateDiscountedPrice(product.Price);
            }
            return product;
        }


        private IQueryable<Product> ApplyIncludes(IQueryable<Product> query, ProductQueryOption queryOption)
        {
            if (queryOption.IncludeReviews)
                query = query.Include(a => a.Reviews);

            if (queryOption.IncludeDiscounts)
                query = query.Include(a => a.Product_Discounts)
                    .ThenInclude(a => a.Discount);

            return query;
        }


        public async Task<Product> Get(Guid id, ProductQueryOption? queryOption = null)
        {
            var query = _dbSet.AsQueryable();

            if (queryOption != null)
            {
                ApplyIncludes(query, queryOption);
                if (queryOption.ReviewsAccepted)
                {
                    //query = query.Select(a => new Product
                    //{
                    //    Title = a.Title,
                    //    Book = a.Book,
                    //    Categories = a.Categories,
                    //    CreateBy = a.CreateBy,
                    //    CreateDate = a.CreateDate,
                    //    DeleteDate = a.DeleteDate,
                    //    DeletedBy = a.DeletedBy,
                    //    DescriptionHtml = a.DescriptionHtml,
                    //    DiscountedPrice = a.DiscountedPrice,
                    //    EBook = a.EBook,
                    //    Favorites = a.Favorites,
                    //    Id = a.Id,
                    //    ImageName = a.ImageName,
                    //    IsDeleted = a.IsDeleted,
                    //    LastModifiedBy = a.LastModifiedBy,
                    //    LastModifiedDate = a.LastModifiedDate,
                    //    NumberOfInventory = a.NumberOfInventory,
                    //    Price = a.Price,
                    //    ProductType = a.ProductType,
                    //    Product_Discounts = a.Product_Discounts,
                    //    SellCount = a.SellCount,
                    //    Reviews = a.Reviews.Where(a => a.IsAccepted),
                    //});
                }

                query = query.Select(q => MapToProduct(q, queryOption.ReviewsAccepted, queryOption.IncludeDiscounts));
            
            }

            Product? product = await query.FirstOrDefaultAsync(a => a.Equals(id));

            if (product == null)
                throw new NotFoundException($"Products with id '{id}' not found");

            return product;
        }

        public async Task<Product> GetByTitle(string title, ProductQueryOption? queryOption = null)
        {
            var query = _dbSet.AsQueryable();

            if (queryOption != null)
            {
                ApplyIncludes(query, queryOption);
                if (queryOption.ReviewsAccepted)
                {
                    //query = query.Select(a => new Product
                    //{
                    //    Title = a.Title,
                    //    Book = a.Book,
                    //    Categories = a.Categories,
                    //    CreateBy = a.CreateBy,
                    //    CreateDate = a.CreateDate,
                    //    DeleteDate = a.DeleteDate,
                    //    DeletedBy = a.DeletedBy,
                    //    DescriptionHtml = a.DescriptionHtml,
                    //    DiscountedPrice = a.DiscountedPrice,
                    //    EBook = a.EBook,
                    //    Favorites = a.Favorites,
                    //    Id = a.Id,
                    //    ImageName = a.ImageName,
                    //    IsDeleted = a.IsDeleted,
                    //    LastModifiedBy = a.LastModifiedBy,
                    //    LastModifiedDate = a.LastModifiedDate,
                    //    NumberOfInventory = a.NumberOfInventory,
                    //    Price = a.Price,
                    //    ProductType = a.ProductType,
                    //    Product_Discounts = a.Product_Discounts,
                    //    SellCount = a.SellCount,
                    //    Reviews = a.Reviews.Where(a => a.IsAccepted),
                    //});
                }

                query = query.Select(q => MapToProduct(q, queryOption.ReviewsAccepted, queryOption.IncludeDiscounts));
            }

            Product? product = await query.FirstOrDefaultAsync(a => a.Title == title);

            if (product == null)
                throw new NotFoundException($"Product with title '{title}' not found");

            return product;
        }




    }
}

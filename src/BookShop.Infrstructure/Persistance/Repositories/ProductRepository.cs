using AutoMapper.QueryableExtensions;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BookShop.Application.Extensions;
using System.Linq.Expressions;
using System.Data.Common;
using BookShop.Domain.Common.Entity;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class ProductRepository : CrudRepository<Product, Guid>, IProductRepository
    {
        public ProductRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

        public static Product ReadProductFromDbData(DbDataReader reader)
        {
            return new Product()
            {
                Id = reader.GetFieldValue<Guid>("Id"),
                CreateBy = reader.GetFieldValue<string>("CreateBy"),
                CreateDate = reader.GetFieldValue<DateTime>("CreateDate"),
                DeleteDate = reader.IsDBNull("DeleteDate") ? null : reader.GetFieldValue<DateTime>("DeleteDate"),
                DeletedBy = reader.IsDBNull("DeletedBy") ? null : reader.GetFieldValue<string>("DeletedBy"),
                DescriptionHtml = reader.GetFieldValue<string>("DescriptionHtml"),
                DiscountedPrice = reader.IsDBNull("DiscountedPrice") ? null : reader.GetFieldValue<float>("DiscountedPrice"),
                ImageName = reader.GetFieldValue<string?>("ImageName"),
                IsDeleted = reader.GetFieldValue<bool>("IsDeleted"),
                LastModifiedBy = reader.GetFieldValue<string>("LastModifiedBy"),
                LastModifiedDate = reader.GetFieldValue<DateTime>("LastModifiedDate"),
                NumberOfInventory = reader.GetFieldValue<int>("NumberOfInventory"),
                Price = reader.GetFieldValue<int>("Price"),
                ProductType = reader.GetFieldValue<ProductType>("ProductType"),
                ReviewsAcceptedAverageScore = reader.GetFieldValue<float>("ReviewsAcceptedAverageScore"),
                SellCount = reader.GetFieldValue<int>("SellCount"),
                Title = reader.GetFieldValue<string>("Title")
            };
        }

        public static Expression<Func<Product, Product>> ProductMapExpression = p => new Product
        {
            Title = p.Title,
            Book = p.Book,
            //Product_Categories = p.Product_Categories,
            Category = p.Category,
            CategoryId = p.CategoryId,
            CreateBy = p.CreateBy,
            CreateDate = p.CreateDate,
            DeleteDate = p.DeleteDate,
            DeletedBy = p.DeletedBy,
            DescriptionHtml = p.DescriptionHtml,
            DiscountedPrice = p.Product_Discounts != null ?
                p.GetDiscountedPrice(p.Product_Discounts.Select(d => d.Discount).OrderBy(d => d.Priority).FirstOrDefault(d => true)) : 0f,
            EBook = p.EBook,
            Favorites = p.Favorites,
            Id = p.Id,
            ImageName = p.ImageName,
            IsDeleted = p.IsDeleted,
            LastModifiedBy = p.LastModifiedBy,
            LastModifiedDate = p.LastModifiedDate,
            NumberOfInventory = p.NumberOfInventory,
            Price = p.Price,
            ProductType = p.ProductType,
            Product_Discounts = p.Product_Discounts,
            SellCount = p.SellCount,
            Reviews = p.Reviews,
        };

        public class ProductQueryObject : Product
        {
            public float FinalPrice { get; set; }
            public float ReviewsAcceptedAverageScore { get; set; }
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




        public async Task<Product> GetByTitle(string title, ProductQueryOption? queryOption = null)
        {
            var query = _dbSet.AsQueryable();

            if (queryOption != null)
            {
                ApplyIncludes(query, queryOption);

                if (queryOption.ProductType != null)
                    query = query.Where(a => a.ProductType == queryOption.ProductType);

                query = query.Select(ProductMapExpression);
            }

            Product? product = await query.FirstOrDefaultAsync(a => a.Title == title);

            if (product == null)
                throw new NotFoundException($"Product with title '{title}' not found");

            return product;
        }




        public async Task<Product> GetWithQuery(Guid id, ProductQueryOption? queryOption)
        {
            var queryString = $"""

                    Select Top(1) P.Id , P.Title , P.Price , P.DescriptionHtml , P.ImageName , P.NumberOfInventory, P.SellCount, P.ProductType,
                   		[P].[CreateDate], [P].LastModifiedDate,[P].[CreateBy], P.LastModifiedBy,P.DeleteDate, P.DeletedBy, P.IsDeleted,
                   		dbo.CalculateDiscounterPrice(P.Price , D.DiscountPrice , D.DiscountPercent) As [DiscountedPrice],
                   		CAST(ISNULL(dbo.CalculateDiscounterPrice(P.Price , D.DiscountPrice , D.DiscountPercent), P.Price) As Float(24))  As [FinalPrice],
                   		ISNULL([r].[AverageScore] , 0.0) As [ReviewsAverageScore],
                		ISNULL([r].[AcceptedAverageScore] , 0.0 ) As [ReviewsAcceptedAverageScore],
                		[D].[Priority]
                     From Products As [p]
                     Left Join (Select * From dbo.GetValidProduct_Discounts()) As D On [p].Id = [D].ProductId
                     Left Join 
                     (
                	        Select R.ProductId ,
                   		    Cast(Avg(CAST([r].Score As real)) As real) As [AverageScore] ,
                   		    CAST(Avg(CAST((Case When [r].IsAccepted = 'True' Then [r].Score End) As float(24))) As real) As [AcceptedAverageScore] 
                	        From Reviews As R
                	        Group By R.ProductId 
                     ) As [r] On [r].[ProductId] = [p].[Id]
                     Where [p].Id = '{id}'
                     Order By [D].[Priority]

                """;

            Product product = null;
            using (var conn = _dbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = queryString;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    product = ReadProductFromDbData(reader);
                }
            }

            return product;
        }








        public async Task<Product> Get(Guid id, ProductQueryOption? queryOption)
        {
            var query = _dbSet.AsQueryable();

            if (queryOption != null)
            {
                ApplyIncludes(query, queryOption);

                if (queryOption.ProductType != null)
                {
                    query = query.Where(a => a.ProductType == queryOption.ProductType);
                }

                //    DiscountedPrice = queryOption.IncludeDiscounts == false ? a.DiscountedPrice :
                //        a.Product_Discounts != null ?
                //            a.GetDscountedPrice(a.Product_Discounts.Select(d => d.Discount).OrderBy(d => d.Priority).FirstOrDefault(d => true)) : 0f,

                //    //DiscountedPrice = a.Product_Discounts != null ? 
                //    //a.Product_Discounts.Select(d => d.Discount).OrderBy(d => d.Priority).FirstOrDefault(d => true).CalculateDiscountedPrice(a.Price) : 0f ,

                //    //DiscountedPrice = a.Product_Discounts != null ? a.GetDscountedPrice(a.Price) : 0f,

                //    //DiscountedPrice = a.GetDscountedPrice(a.Price),

                //    //DiscountedPrice = a.Product_Discounts != null ? 
                //    //a.GetDscountedPrice(a.Product_Discounts.Select(d => d.Discount).OrderBy(d => d.Priority).FirstOrDefault(d => true)) : 0f,

                //    //DiscountedPrice = DiscountePriceMap(a),

                //    //DiscountedPrice = Asd(ref a),

                //    //DiscountedPrice = expression.Compile().Invoke(a),

                //    //DiscountedPrice = a.GetDscountedPrice(a.Product_Discounts),

                //    //DiscountedPrice = a.Product_Discounts != null ? a.GetDscountedPrice(a.Product_Discounts.ToList()) : 0f,

                query = query.Select(ProductMapExpression);

            }

            Product? product = await query.FirstOrDefaultAsync(a => a.Id.Equals(id));

            if (product == null)
                throw new NotFoundException($"Products with id '{id}' not found");

            return product;
        }





        public async Task<IEnumerable<Product>> GetAll(ProductQueryOption queryOption, Paging? paging = null, ProductSortingOrder? productSortingOrder = null)
        {
            paging = paging ?? new Paging();
            var query = _dbSet.AsQueryable();
            query = ApplyIncludes(query, queryOption);
            query = query.Select(ProductMapExpression);


            if (queryOption.StartPrice > 0)
                query = query.Where(p => p.GetDiscountedPrice(p.Product_Discounts.Select(d => d.Discount).OrderBy(d => d.Priority).FirstOrDefault(d => true)) >= queryOption.StartPrice);
            //query = query.Where(q => (q.DiscountedPrice > 0 && q.DiscountedPrice >= queryOption.StartPrice) || q.Price >= queryOption.StartPrice);
            //query = query.Where(q => q.DiscountedPrice >= queryOption.StartPrice);

            if (queryOption.EndPrice > 0)
                query = query.Where(q => q.DiscountedPrice != null ? q.DiscountedPrice <= queryOption.EndPrice : q.Price <= queryOption.EndPrice);

            if (queryOption.ProductType != null)
                query = query.Where(q => q.ProductType == queryOption.ProductType);

            if (productSortingOrder != null)
            {
                bool sorted = false;

                query = ApplyBaseSort(query, ref sorted, productSortingOrder.Value);
            }

            //var a = newQuery.AsEnumerable();

            //return a;

            return query.AsEnumerable();
        }





        public async Task<PaginatedEntities<Product>> GetAllWithQuery(ProductQueryOption queryOption, Paging? paging = null, ProductSortingOrder? sortingOrder = null)
        {
            #region paging
            string? pagingQuery = paging == null ? null :
                $"OFFSET ({paging.PageNumber} - 1) * {paging.ItemsInPage} ROWS FETCH NEXT {paging.ItemsInPage} ROWS ONLY";
            #endregion


            #region joins
            string? ValidDiscountsCTE = queryOption.IncludeDiscounts == false ? null : """
                WITH ValidDiscounts As 
                (
                	SELECT 
                        pd.ProductId, d.DiscountPercent, d.DiscountPrice, d.[Priority],
                        ROW_NUMBER() OVER (PARTITION BY pd.ProductId ORDER BY d.[Priority]) AS [PrioritySortOrder]
                    FROM 
                        Product_Discounts pd
                    JOIN 
                        Discounts d ON pd.DiscountId = d.Id
                	Where 
                		(d.MaximumUseCount Is Null Or d.MaximumUseCount > d.UsedCount) And
                		(d.StartDate Is Null Or d.StartDate < GETDATE()) And
                		(d.EndDate Is Null Or d.EndDate > GETDATE()) And
                		(d.EndDate Is Null Or d.EndDate > GETDATE()) And
                		(d.DiscountPrice Is Not Null Or d.DiscountPercent Is Not Null)
                )    
                """;

            string? validDicountsJoin = queryOption.IncludeDiscounts == false ? null : """
                LEFT JOIN 
                    ValidDiscounts vd ON p.Id = vd.ProductId AND vd.PrioritySortOrder = 1
                """;

            string? productsWithReviewAverageScoreJoin = queryOption.IncludeDiscounts == false ? null : """
                LEFT JOIN 
                    (Select * From dbo.GetProductsWithReviewsAverageScore()) As [r] On [r].[ProductId] = [p].[Id]
                """;
            #endregion


            #region columns
            string discountePriceColumn = queryOption.IncludeDiscounts == false ? "dbo.CalculateDiscounterPrice(P.Price , Null, Null)" :
                "dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent)";

            string finalPriceColumn = $"CAST(ISNULL({discountePriceColumn} , P.Price) As real)";

            string acceptedAverageScoreColumn = queryOption.IncludeReviews ? "CAST(ISNULL([r].[AcceptedAverageScore] , 0.0 ) As real)" :
                "CAST(ISNULL(Null, 0.0 ) As real)";
            #endregion


            #region filters
            string? TitleFilter = string.IsNullOrEmpty(queryOption.Title) ? null :
              $"[p].[Title] LIKE '%{queryOption.Title}%' And";

            string? startPriceFilter = queryOption.StartPrice > 0 ?
                $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) >= {queryOption.StartPrice} And" : null;

            string? endPriceFilter = queryOption.EndPrice > 0 ?
               $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) <= {queryOption.EndPrice} And" : null;

            string? productTypeFilter = queryOption.ProductType == null ? null :
                $"[p].[ProductType] = {(int)queryOption.ProductType.Value} And";

            string? productIsAvailableFilter = queryOption.Available != null ?
                (queryOption.Available.Value ? "[p].[NumberOfInventory] > 0 And" : "[p].[NumberOfInventory] <= 0 And") : null;

            string? averageScoreFilter = (queryOption.AverageScore != null && queryOption.AverageScore > 0 && queryOption.AverageScore <= 5) == false ? null :
                $"{acceptedAverageScoreColumn} >= {queryOption.AverageScore} And {acceptedAverageScoreColumn} < {queryOption.AverageScore + 1} And";
            #endregion


            #region sort
            string? orderByQuery = null;

            if (sortingOrder != null)
                switch (sortingOrder.Value)
                {
                    case ProductSortingOrder.Newest:
                        orderByQuery = ApplyBaseSort("[p]", sortingOrder.Value); break;

                    case ProductSortingOrder.Oldest:
                        orderByQuery = ApplyBaseSort("[p]", sortingOrder.Value); break;

                    case ProductSortingOrder.HighestPrice:
                        orderByQuery = $"Order By {finalPriceColumn} Desc"; break;

                    case ProductSortingOrder.LowestPrice:
                        orderByQuery = $"Order By {finalPriceColumn}"; break;

                    case ProductSortingOrder.HighestDiscount:
                        orderByQuery = $"Order By {discountePriceColumn} Desc"; break;

                    case ProductSortingOrder.LowestDiscount:
                        orderByQuery = $"Order By {discountePriceColumn}"; break;

                    case ProductSortingOrder.HighestSellCount:
                        orderByQuery = $"Order By [p].[SellCount] Desc"; break;

                    case ProductSortingOrder.LowestSellCount:
                        orderByQuery = $"Order By [p].[SellCount]"; break;

                    case ProductSortingOrder.AlphabetDesc:
                        orderByQuery = $"Order By [p].[Title] Desc"; break;

                    case ProductSortingOrder.AlphabetAsce:
                        orderByQuery = $"Order By [p].[Title]"; break;

                    default:
                        break;
                }

            if (orderByQuery == null && paging != null)
                orderByQuery = "Order By Id";
            #endregion


            string queryString = $"""
                {ValidDiscountsCTE}
                SELECT 
                    p.Id ,p.Title , p.Price , P.DescriptionHtml , P.ImageName , P.NumberOfInventory , P.SellCount , P.ProductType,
                    [P].[CreateDate] , [P].LastModifiedDate , [P].[CreateBy] , P.LastModifiedBy , P.DeleteDate , P.DeletedBy , P.IsDeleted,
                    {discountePriceColumn} As [DiscountedPrice],
                    {finalPriceColumn} As [FinalPrice] ,
                    {acceptedAverageScoreColumn} As [ReviewsAcceptedAverageScore],
                    COUNT(*) OVER() AS [TotalItemCount]
                FROM 
                    Products p
                {validDicountsJoin}
                {productsWithReviewAverageScoreJoin}
                Where   
                    1 = 1 And
                    {startPriceFilter} {endPriceFilter} {productTypeFilter} {TitleFilter} 
                    {productIsAvailableFilter} {averageScoreFilter}
                {orderByQuery}
                {pagingQuery}
                """;

            queryString = queryString.RemoveLastOccurrenceOfWord("And");

            Product product;
            int totalItemCount = -1;
            List<Product> products = new List<Product>();
            using (var conn = _dbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = queryString;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if(totalItemCount < 0)
                    {
                        totalItemCount = reader.GetInt32("TotalItemCount");
                    }
                    product = ReadProductFromDbData(reader);
                    products.Add(product);
                }
            }

            return new PaginatedEntities<Product>(products , paging , totalItemCount);
        }

        public async Task<bool> IsExist(string title)
        {
            return await _dbSet.AnyAsync(a => a.Title == title);
        }

        public async Task<bool> IsExist(string title, Guid exceptId)
        {
            return await _dbSet.AnyAsync(a => a.Title == title && a.Id != exceptId);
        }

    }
}

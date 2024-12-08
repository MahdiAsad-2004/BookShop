﻿using BookShop.Application.Common.Dtos;
using BookShop.Application.Extensions;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using BookShop.Infrstructure.Persistance.QueryOptions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class BookRepository : CrudRepository<Book, Guid>, IBookRepository
    {
        private readonly Product _product = new Product();
        private readonly Book _book = new Book();

        public BookRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        { }




        public static Book ReadBookFromDbData(DbDataReader reader)
        {
            Product product = new Product()
            {
                Id = reader.GetFieldValue<Guid>("p_Id"),
                CreateBy = reader.GetFieldValue<string>("p_CreateBy"),
                CreateDate = reader.GetFieldValue<DateTime>("p_CreateDate"),
                DeleteDate = reader.IsDBNull("p_DeleteDate") ? null : reader.GetFieldValue<DateTime>("p_DeleteDate"),
                DeletedBy = reader.IsDBNull("p_DeletedBy") ? null : reader.GetFieldValue<string>("p_DeletedBy"),
                DescriptionHtml = reader.GetFieldValue<string>("p_DescriptionHtml"),
                DiscountedPrice = reader.IsDBNull("p_DiscountedPrice") ? null : reader.GetFieldValue<float>("p_DiscountedPrice"),
                ImageName = reader.GetFieldValue<string>("p_ImageName"),
                IsDeleted = reader.GetFieldValue<bool>("p_IsDeleted"),
                LastModifiedBy = reader.GetFieldValue<string>("p_LastModifiedBy"),
                LastModifiedDate = reader.GetFieldValue<DateTime>("p_LastModifiedDate"),
                NumberOfInventory = reader.GetFieldValue<int>("p_NumberOfInventory"),
                Price = reader.GetFieldValue<int>("p_Price"),
                ProductType = reader.GetFieldValue<ProductType>("p_ProductType"),
                ReviewsAcceptedAverageScore = reader.GetFieldValue<float>("p_ReviewsAcceptedAverageScore"),
                SellCount = reader.GetFieldValue<int>("p_SellCount"),
                Title = reader.GetFieldValue<string>("p_Title"),
            };
            return new Book
            {
                Id = reader.GetGuid("Id"),
                Cover = reader.GetFieldValue<Cover>("Cover"),
                CreateBy = reader.GetString("CreateBy"),
                CreateDate = reader.GetDateTime("CreateDate"),
                Cutting = reader.GetFieldValue<Cutting>("Cutting"),
                DeleteDate = reader.IsDBNull("DeleteDate") ? null : reader.GetFieldValue<DateTime>("DeleteDate"),
                DeletedBy = reader.IsDBNull("DeletedBy") ? null : reader.GetFieldValue<string>("DeletedBy"),
                IsDeleted = reader.GetFieldValue<bool>("IsDeleted"),
                Language = reader.GetFieldValue<Languages>("Language"),
                Edition = reader.IsDBNull("Edition") ? null : reader.GetInt32("Edition"),
                LastModifiedBy = reader.GetFieldValue<string>("LastModifiedBy"),
                LastModifiedDate = reader.GetFieldValue<DateTime>("LastModifiedDate"),
                NumberOfPages = reader.GetInt32("NumberOfPages"),
                PublisherId = reader.GetGuid("PublisherId"),
                PublishYear = reader.GetDateTime("PublishYear"),
                Shabak = reader.IsDBNull("Shabak") ? null : reader.GetString("Shabak"),
                TranslatorId = reader.IsDBNull("TranslatorId") ? null : reader.GetGuid("TranslatorId"),
                WeightInGram = reader.IsDBNull("WeightInGram") ? null : reader.GetFloat("WeightInGram"),
                ProductId = reader.GetGuid("ProductId"),
                Product = product,
            };
        }





        public async Task<Book> Get(Guid id, BookQueryOption queryOption)
        {
            //return await GetWithLinq(id);
            return await GetWithQuery(id, queryOption);
        }






        private async Task<Book> GetWithQuery(Guid id, BookQueryOption queryOption)
        {
            var bookQueryObject = await _dbContext.Books
            .Select(book => new BookQueryResultObject
            {
                Book = book,
                Product = _dbContext.Products.First(a => a.Id == book.ProductId),
                Reviews = queryOption.IncludeReviews == false ? null : _dbContext.Reviews.Where(a => a.ProductId == book.ProductId).ToList(),
                Publisher = queryOption.IncludePublisher == false ? null : _dbContext.Publishers.First(a => a.Id == book.PublisherId),
                Translator = queryOption.IncludeTranslator == false ? null : _dbContext.Translators.FirstOrDefault(a => a.Id == book.TranslatorId),
                Authors = queryOption.IncludeAuthors == false ? null : _dbContext.Authors.ToList(),
                MostPriorityValidDiscount = queryOption.IncludeDiscounts == false ? null : _dbContext.Product_Discounts
                    .Where(pd => pd.ProductId == book.ProductId)
                    .Join(_dbContext.Discounts,
                        pd => pd.DiscountId,
                        d => d.Id,
                        (pd, d) => new Discount
                        {
                            DiscountPercent = d.DiscountPercent,
                            DiscountPrice = d.DiscountPrice,
                            Priority = d.Priority,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                            UsedCount = d.UsedCount,
                            MaximumUseCount = d.MaximumUseCount,
                        })
                    .Where
                    (d =>
                        (d.StartDate == null || d.StartDate <= DateTime.Now) &&
                        (d.EndDate == null || d.EndDate >= DateTime.Now) &&
                        (d.MaximumUseCount == null || d.MaximumUseCount > d.UsedCount) &&
                        (d.DiscountPercent != null || d.DiscountPrice != null)
                    )
                    .OrderBy(d => d.Priority)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(a => a.Book.Id == id);

            if (bookQueryObject == null)
                throw new NotFoundException($"Book with id '{id}' not found");

            return bookQueryObject.MapToBook();
        }




        private async Task<Book> GetWithLinq(Guid id)
        {
            var query = _dbSet.AsQueryable();

            query = query.Include(a => a.Product)
                    .ThenInclude(a => a.Reviews)
                .Include(a => a.Product)
                    .ThenInclude(a => a.Product_Discounts)
                        .ThenInclude(a => a.Discount)
                .Include(a => a.Product)
                    .ThenInclude(a => a.Categories)
                .Include(a => a.Publisher)
                .Include(a => a.Translator)
                .Include(a => a.Authors);

            Book? book = await query.FirstOrDefaultAsync(a => a.Id == id);

            if (book == null)
                throw new NotFoundException($"Book with id '{id}' not found");

            return book;
        }



        public async Task<PaginatedEntities<Book>> GetAll(BookQueryOption queryOption, Paging? paging = null, BookSortingOrder? sortingOrder = null)
        {
            #region paging
            string? pagingQuery = paging == null ? null :
                $"OFFSET ({paging.PageNumber} - 1) * {paging.ItemsInPage} ROWS FETCH NEXT {paging.ItemsInPage} ROWS ONLY";
            #endregion


            #region joins
            string? productValidDiscountsCTE = queryOption.IncludeDiscounts == false || queryOption.IncludeProduct == false ? null : """

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

            string? productValidDicountsJoin = queryOption.IncludeDiscounts == false || queryOption.IncludeProduct == false ? null : """
                LEFT JOIN 
                    ValidDiscounts vd ON p.Id = vd.ProductId AND vd.PrioritySortOrder = 1
                """;
            string? productsJoin = queryOption.IncludeProduct == false ? null : """
                INNER JOIN 
                    Products [p] On [b].[ProductId] = [p].[Id]
                """;
            string? productsWithReviewAverageScoreJoin = queryOption.IncludeReviews == false || queryOption.IncludeProduct == false ? null : """
                LEFT JOIN 
                    (Select * From dbo.GetProductsWithReviewsAverageScore()) As [r] On [r].[ProductId] = [p].[Id]
                """;
            #endregion


            #region columns
            string discountePriceColumn = queryOption.IncludeDiscounts == false ?
                "dbo.CalculateDiscounterPrice(P.Price , Null, Null)" :
                "dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent)";

            string finalPriceColumn = $"CAST(ISNULL({discountePriceColumn} , P.Price) As real)";

            string acceptedAverageScoreColumn = queryOption.IncludeReviews ?
                "CAST(ISNULL([r].[AcceptedAverageScore] , 0.0 ) As real)" :
                "CAST(ISNULL(Null, 0.0 ) As real)";

            string? productColumns = queryOption.IncludeProduct == false ? null : $"""
                    p.Id As [p_Id] ,p.Title As [p_Title], p.Price As [p_Price] , P.DescriptionHtml As [p_DescriptionHtml] , P.ImageName As [p_ImageName],
                    P.NumberOfInventory As [p_NumberOfInventory], P.SellCount As [p_SellCount], P.ProductType As [p_ProductType],
                    [P].[CreateDate] As [p_CreateDate], [P].LastModifiedDate As [p_LastModifiedDate], [P].[CreateBy] As [p_CreateBy],
                    P.LastModifiedBy As [p_LastModifiedBy], P.DeleteDate As [p_DeleteDate], P.DeletedBy As [p_DeletedBy], P.IsDeleted As [p_IsDeleted],
                    {discountePriceColumn} As [p_DiscountedPrice],
                    {finalPriceColumn} As [p_FinalPrice] ,
                    {acceptedAverageScoreColumn} As [p_ReviewsAcceptedAverageScore]
                """;
            #endregion


            #region filters
            string? productStartPriceFilter = queryOption.Product_StartPrice > 0 && queryOption.IncludeProduct ?
           $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) >= {queryOption.Product_StartPrice} And" : null;

            string? productEndPriceFilter = queryOption.Product_EndPrice > 0 && queryOption.IncludeProduct ?
               $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) <= {queryOption.Product_EndPrice} And" : null;

            string? productIsAvailableFilter = queryOption.Product_Available != null && queryOption.IncludeProduct ?
                (queryOption.Product_Available.Value ? "[p].[NumberOfInventory] > 0 And" : "[p].[NumberOfInventory] <= 0 And") : null;

            string? productAverageScoreFilter =
                (queryOption.Product_AverageScore != null && queryOption.Product_AverageScore > 0 && queryOption.Product_AverageScore <= 5) && queryOption.IncludeProduct ?
                $"{acceptedAverageScoreColumn} >= {queryOption.Product_AverageScore} And {acceptedAverageScoreColumn} < {queryOption.Product_AverageScore + 1} And" : null;

            string? startPublishYearFilter = queryOption.StartPublishYear == null || queryOption.StartPublishYear > DateTime.Now ? null :
                $"[b].[PublishYear] >= '{queryOption.StartPublishYear.Value}' And";

            string? endPublishYearFilter = queryOption.EndPublishYear == null || queryOption.StartPublishYear < DateTime.Now ? null :
                $"[b].[PublishYear] <= '{queryOption.EndPublishYear.Value}' And";
            #endregion


            #region sort
            string? orderByQuery = null;
            if (sortingOrder != null)
            {
                switch (sortingOrder.Value)
                {
                    case BookSortingOrder.Newest:
                        orderByQuery = ApplyBaseSort("[b]", sortingOrder.Value); break;

                    case BookSortingOrder.Oldest:
                        orderByQuery = ApplyBaseSort("[b]", sortingOrder.Value); break;

                    case BookSortingOrder.Product_HighestPrice:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By {finalPriceColumn} Desc"; break;

                    case BookSortingOrder.Product_LowestPrice:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By {finalPriceColumn}"; break;

                    case BookSortingOrder.Product_HighestDiscount:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By {discountePriceColumn} Desc"; break;

                    case BookSortingOrder.Product_LowestDiscount:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By {discountePriceColumn}"; break;

                    case BookSortingOrder.Product_HighestSellCount:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By [p].[SellCount] Desc"; break;

                    case BookSortingOrder.Product_LowestSellCount:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By [p].[SellCount]"; break;

                    case BookSortingOrder.Product_AlphabetDesc:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By [p].[Title] Desc"; break;

                    case BookSortingOrder.Product_AlphabetAsce:
                        if (queryOption.IncludeProduct)
                            orderByQuery = $"Order By [p].[Title]"; break;

                    case BookSortingOrder.PublishYearAsce:
                        orderByQuery = $"Order By [b].[PublishYear]"; break;

                    case BookSortingOrder.PublishYearDesc:
                        orderByQuery = $"Order By [b].[PublishYear] Desc"; break;
                }
            }
            if (orderByQuery == null && paging != null)
                orderByQuery = "Order By Id";
            #endregion


            string queryString = $"""

                {productValidDiscountsCTE}
                SELECT
                
                    b.Id , b.NumberOfPages, b.Cover , b.Cutting , b.Language , b.Shabak , b.PublishYear , b.WeightInGram,
                    b.Edition , b.PublisherId, b.TranslatorId , [b].ProductId , [b].[CreateDate] , b.LastModifiedDate ,
                    b.[CreateBy] , b.LastModifiedBy , b.DeleteDate , b.DeletedBy , b.IsDeleted ,
                    {productColumns} , 
                    COUNT(*) OVER() AS [TotalItemCount]
                FROM 
                    Books [b]
                {productsJoin}
                {productValidDicountsJoin}
                {productsWithReviewAverageScoreJoin}
                Where   
                    1 = 1 And
                    {productStartPriceFilter} {productEndPriceFilter} {productIsAvailableFilter} {productAverageScoreFilter}
                    {startPublishYearFilter} {endPublishYearFilter}
                {orderByQuery}
                {pagingQuery}
                """;

            queryString = queryString.RemoveLastOccurrenceOfWord("And");

            Book book;
            int totalItemCount = -1;
            List<Book> books = new List<Book>();
            using (var conn = _dbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = queryString;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (totalItemCount < 0)
                    {
                        totalItemCount = reader.GetInt32("TotalItemCount");
                    }
                    book = ReadBookFromDbData(reader);
                    books.Add(book);
                }
            }

            return new PaginatedEntities<Book>(books, paging, totalItemCount);
        }





    }
}

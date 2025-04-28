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
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class BookRepository : CrudRepository<Book, Guid>, IBookRepository
    {
        //private readonly Product _product = new Product();
        //private readonly Book _book = new Book();

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
                ImageName = reader.GetFieldValue<string?>("p_ImageName"),
                IsDeleted = reader.GetFieldValue<bool>("p_IsDeleted"),
                LastModifiedBy = reader.GetFieldValue<string>("p_LastModifiedBy"),
                LastModifiedDate = reader.GetFieldValue<DateTime>("p_LastModifiedDate"),
                NumberOfInventory = reader.GetFieldValue<int>("p_NumberOfInventory"),
                Price = reader.GetFieldValue<int>("p_Price"),
                ProductType = reader.GetFieldValue<ProductType>("p_ProductType"),
                ReviewsAcceptedAverageScore = reader.GetFieldValue<float>("p_ReviewsAcceptedAverageScore"),
                SellCount = reader.GetFieldValue<int>("p_SellCount"),
                Title = reader.GetFieldValue<string>("p_Title"),
                CategoryId = reader.IsDBNull("p_CategoryId") ? null : reader.GetFieldValue<Guid>("p_CategoryId"),
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
                Language = reader.GetFieldValue<Language>("Language"),
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
            if (id == Guid.Empty)
                throw new NotFoundException($"Book with id '{id}' not found !");
            //return await GetWithLinq(id);
            return await GetWithQuery(id, queryOption);
        }






        private async Task<Book> GetWithQuery(Guid id, BookQueryOption queryOption)
        {
            var bookQueryObject = await _dbContext.Books.AsNoTracking()
            .Select(book => new BookQueryResultObject
            {
                Book = book,
                Product = queryOption.IncludeProduct == false ? null : _dbContext.Products.First(a => a.Id == book.ProductId),
                Reviews = queryOption.IncludeReviews == false ? null : _dbContext.Reviews.Include(a => a.User).Where(a => a.ProductId == book.ProductId).ToList(),
                Publisher = queryOption.IncludePublisher == false ? null : _dbContext.Publishers.First(a => a.Id == book.PublisherId),
                Translator = queryOption.IncludeTranslator == false ? null : _dbContext.Translators.FirstOrDefault(a => a.Id == book.TranslatorId),
                Author_Books = queryOption.IncludeAuthors == false ? null : _dbContext.Author_Books.Include(a => a.Author).Where(a => a.BookId == book.Id).ToArray(),
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
                        .Where(
                            d =>
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
                        .ThenInclude(a => a.Category)
                .Include(a => a.Publisher)
                .Include(a => a.Translator)
                .Include(a => a.Author_Books)
                    .ThenInclude(a => a.Author);

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
                ),    
                """;

            string? categoryCTE = queryOption.CategoryId == null ? null :
                $"""
                WITH CategoryIds AS 
                (
                    SELECT Id FROM Categories
                    WHERE Id = '{queryOption.CategoryId}'
                    UNION ALL
                    SELECT c.Id FROM Categories c
                    INNER JOIN CategoryIds rc ON c.ParentId = rc.Id
                ),
                """;

            string CTES = $"{productValidDiscountsCTE}{categoryCTE}";
            if (productValidDiscountsCTE != null && categoryCTE != null)
            {
                CTES = TextExtensions.RemoveLastOccurrenceOfWord(CTES, "WITH");
            }
            CTES = TextExtensions.RemoveLastOccurrenceOfWord(CTES, ",");

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
            string? categoryIdsJoin = queryOption.CategoryId == null ? null : """
                INNER JOIN
                    CategoryIds On p.CategoryId = CategoryIds.Id
                """;
            string? authorIdsJoin = queryOption.AuthorId == null ? null : $"""
                INNER JOIN 
                    Author_Books On [b].[Id] = Author_Books.[BookId]
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
                    P.NumberOfInventory As [p_NumberOfInventory], P.SellCount As [p_SellCount], P.ProductType As [p_ProductType],[p].CategoryId As [p_CategoryId],
                    [P].[CreateDate] As [p_CreateDate], [P].LastModifiedDate As [p_LastModifiedDate], [P].[CreateBy] As [p_CreateBy],
                    P.LastModifiedBy As [p_LastModifiedBy], P.DeleteDate As [p_DeleteDate], P.DeletedBy As [p_DeletedBy], P.IsDeleted As [p_IsDeleted],
                    {discountePriceColumn} As [p_DiscountedPrice],
                    {finalPriceColumn} As [p_FinalPrice] ,
                    {acceptedAverageScoreColumn} As [p_ReviewsAcceptedAverageScore]
                """;
            #endregion


            #region filters
            string? productTitleFilter = string.IsNullOrEmpty(queryOption.Product_Title) ? null :
                $"[p].[Title] LIKE '%{queryOption.Product_Title}%' And";

            string? productStartPriceFilter = queryOption.Product_StartPrice > 0 && queryOption.IncludeProduct ?
                $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) >= {queryOption.Product_StartPrice} And" : null;

            string? productEndPriceFilter = queryOption.Product_EndPrice > 0 && queryOption.IncludeProduct ?
               $"ISNULL(dbo.CalculateDiscounterPrice(P.Price , vd.DiscountPrice , vd.DiscountPercent), P.Price) <= {queryOption.Product_EndPrice} And" : null;

            string? productIsAvailableFilter = queryOption.Product_IsAvailable != null && queryOption.IncludeProduct ?
                (queryOption.Product_IsAvailable.Value ? "[p].[NumberOfInventory] > 0 And" : "[p].[NumberOfInventory] <= 0 And") : null;

            string? productAverageScoreFilter =
                (queryOption.Product_AverageScore != null && queryOption.Product_AverageScore > 0 && queryOption.Product_AverageScore <= 5) && queryOption.IncludeProduct ?
                $"{acceptedAverageScoreColumn} >= {queryOption.Product_AverageScore} And {acceptedAverageScoreColumn} < {queryOption.Product_AverageScore + 1} And" : null;

            string? startPublishYearFilter = queryOption.StartPublishYear == null || queryOption.StartPublishYear > DateTime.Now ? null :
                $"[b].[PublishYear] >= '{queryOption.StartPublishYear.Value}' And";

            string? endPublishYearFilter = queryOption.EndPublishYear == null || queryOption.StartPublishYear < DateTime.Now ? null :
                $"[b].[PublishYear] <= '{queryOption.EndPublishYear.Value}' And";

            string? coverFilter = queryOption.Cover == null ? null :
                $"[b].[Cover] = {(int)queryOption.Cover} And";

            string? languageFilter = queryOption.Language == null ? null :
                $"[b].[Language] = {(int)queryOption.Language} And";

            string? cuttingFilter = queryOption.Cutting == null ? null :
                $"[b].[Cutting] = {(int)queryOption.Cutting} And";

            string? authorFilter = queryOption.AuthorId == null ? null :
                $"Author_Books.[AuthorId] = '{queryOption.AuthorId}' And";

            string? publisherFilter = queryOption.PublisherId == null ? null :
                $"[b].[PublisherId] = '{queryOption.PublisherId}' And";

            string? translatorFilter = queryOption.TranslatorId == null ? null :
                $"[b].[TranslatorId] = '{queryOption.TranslatorId}' And";
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
                orderByQuery = "Order By [b].[Id]";
            #endregion


            string queryString = $"""

                {CTES}
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
                {categoryIdsJoin}
                {authorIdsJoin}
                Where   
                    1 = 1 And
                    {productStartPriceFilter} {productEndPriceFilter} {productIsAvailableFilter} {productAverageScoreFilter}
                    {startPublishYearFilter} {endPublishYearFilter} {coverFilter} {languageFilter} {cuttingFilter} {authorFilter}
                    {publisherFilter} {translatorFilter} {productTitleFilter}
                {orderByQuery}
                {pagingQuery}
                """;

            queryString = queryString.RemoveLastOccurrenceOfWord("And");

            Book book;
            int totalItemCount = -1;
            List<Book> books = new List<Book>();
            using (var conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
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
                await conn.CloseAsync();
            }

            return new PaginatedEntities<Book>(books, paging, totalItemCount);
        }


        public async Task Add(Book book, Product product,Guid[] authorIds)
        {
            DateTime dateTime = DateTime.UtcNow;
            product.Id = Guid.NewGuid();
            product.CreateDate = product.LastModifiedDate = dateTime;
            product.CreateBy = product.LastModifiedBy = _currentUser.GetId();
            //---------------------------------------------------------------------
            book.Id = product.Id;
            book.CreateDate = book.LastModifiedDate = dateTime;
            book.CreateBy = book.LastModifiedBy = _currentUser.GetId();
            //---------------------------------------------------------------------
            List<Author_Book> author_Books = new List<Author_Book>();
            if (authorIds != null)
            {
                foreach (Guid authorId in authorIds)
                {
                    author_Books.Add(new Author_Book
                    {
                        CreateBy = _currentUser.GetId(),
                        CreateDate = dateTime,
                        LastModifiedBy = _currentUser.GetId(),
                        LastModifiedDate = dateTime,
                        AuthorId = authorId,
                        Book = book,
                        BookId = book.Id,
                        Id = Guid.NewGuid(),
                    });
                }
            }
            //---------------------------------------------------------------------
            book.Product = product;
            book.Author_Books = author_Books;
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            await book.PublishAllDomainEventsAndClear(_domainEventPublisher);
        }



    }
}

using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class Author_EBookRepository : CrudRepository<Author_EBook, Guid> , IAuthor_EBookRepository
    {
        public Author_EBookRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task InsertNewOnesAndDeleteAnothers(Guid ebookId, Guid[] authorIds)
        {
            string joinedIds = string.Join(",", authorIds.Select(num => $"('{num}')"));
            string joinedIdsWithComma = string.Join(",", authorIds.Select(num => $"'{num}'"));
            string now = DateTime.UtcNow.ToString();
            string commandText = $"""
                BEGIN TRANSACTION

                Delete Author_EBooks 
                WHERE Author_EBooks.[AuthorId] NOT IN ({joinedIdsWithComma})

                INSERT INTO Author_EBooks(Id,AuthorId,EBookId,CreateBy,CreateDate,IsDeleted,LastModifiedBy,LastModifiedDate)
                SELECT 
                NEWID() As [Id] ,
                AuthorIds.[Id] AS [AuthorId], 
                '{ebookId}' AS [EBookId] , 
                '{_currentUser.Id}' AS [CreateBy] ,
                '{now}' AS [CreateDate] ,
                0 AS [IsDeleted] ,
                '{_currentUser.Id}' AS [LastModifiedBy],
                '{now}' AS [LastModifiedDate]
                FROM (VALUES {joinedIds}) AS AuthorIds(Id)
                WHERE AuthorIds.[Id] NOT IN
                (
                	SELECT AuthorId FROM Author_EBooks
                	WHERE EBookId = '{ebookId}'
                )

                COMMIT TRANSACTION;
                """;
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }



    }
}

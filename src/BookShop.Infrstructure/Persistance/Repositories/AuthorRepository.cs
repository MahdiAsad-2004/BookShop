using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class AuthorRepository : CrudRepository<Author, Guid> , IAuthorRepository
    {
        public AuthorRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

        public async Task<bool> IsExist(string name)
        {
            return await _dbSet.AnyAsync(a => a.Name == name);
        }
        
        public async Task<bool> IsExist(string name, Guid exceptId) 
        {
            return await _dbSet.AnyAsync(a => a.Name == name && a.Id != exceptId);
        }




        public async Task<bool> AreExist(Guid[] ids)
        {
            string joinedIds = string.Join(",", ids.Select(num => $"'{num}'"));
            int existCount = 0;
            
            using(var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = $"""
                        Select Count(Id) From Authors as [a]
                        Where [a].Id In ({joinedIds})
                    """;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    existCount = reader.GetInt32(0);
                }
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
            return existCount == ids.Count();
        }



    }
}

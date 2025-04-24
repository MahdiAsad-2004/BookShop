using BookShop.Domain.Common;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;

using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class UserRepository : CrudRepository<User, Guid>, IUserRepository
    {
        public UserRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        { }


        public async Task<User> GetByNormalizedUsername(string normalizedUsername)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedUsername == normalizedUsername);
            if (user == null)
                throw new NotFoundException($"User with NormalizedUsername ({normalizedUsername}) not found");
            return user;
        }


        public async Task<User> GetByNormalizedEmail(string normalizedEmail)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedEmail == normalizedEmail);
            if (user == null)
                throw new NotFoundException($"User with NormalizedEmail ({normalizedEmail}) not found");
            return user;
        }

        public async Task<User?> GetByNormalizedUsernameOrDefault(string normalizedUsername)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedUsername == normalizedUsername);
            return user;
        }


        //public override Task<IEnumerable<User>> GetAll(UserQueryOption queryOption)
        //{
        //    var query = _dbSet.AsQueryable();
        //    query = _queryOptionOperator.PerformEntityIncludes(queryOption,query);

        //    return Task.FromResult(query.AsEnumerable());
        //}

        //public override async Task<User> Get(Guid key, UserQueryOption queryOption)
        //{
        //    var query = _dbSet.AsQueryable();
        //    query = _queryOptionOperator.PerformEntityIncludes(queryOption, query);

        //    User? user = await query.FirstOrDefaultAsync(a => a.Id == key);

        //    if (user == null)
        //        throw new NotFoundException($"User with id ({key} not found)");

        //    return user;
        //}


        public async Task<(Guid id, string? role)?> Login(string username, string passwordHash)
        {

            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = $"""
                       Select Users.Id , Roles.Name
                       From Users 
                       Left Join Roles On Users.RoleId = Roles.Id
                       Where
                       Users.Username = '{username}' And 
                       Users.PasswordHash = '{passwordHash}'
                    """;
                var reader = await command.ExecuteReaderAsync();
                
                if (reader.HasRows == false)
                    return null;
                
                while (await reader.ReadAsync())
                {
                    Guid id = reader.GetFieldValue<Guid>(0);
                    string role = reader.IsDBNull(1) ? null : reader.GetFieldValue<string>(1);
                    
                    return new (id, role);
                }
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
            return null;
        }

        public async Task<(Guid id, string username, string? role)?> GetUserTokenRequirements(Guid id)
        {
            (Guid id, string username, string? role)? result = null;
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = $"""
                       Select Users.Id ,Users.Username ,Roles.Name
                       From Users 
                       Left Join Roles On Users.RoleId = Roles.Id
                       Where
                       Users.Id = '{id}'
                    """;
                bool read = false;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    read = true;
                    result = (
                            reader.GetFieldValue<Guid>(0),
                            reader.GetFieldValue<string>(1),
                            reader.GetFieldValue<string?>(2)
                        );
                }
                await connection.CloseAsync();
                await connection.DisposeAsync();
                if (read)
                    return result;
            }
            return result;
        }



    }

}

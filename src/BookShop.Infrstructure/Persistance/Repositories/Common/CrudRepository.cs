using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Infrastructure.Persistance.QueryOptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace BookShop.Infrastructure.Persistance.Repositories.Common
{
    internal abstract class CrudRepository<TEntity, TKey> : 
        IDisposable,
        ICrudRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
        where TKey : struct    
    {
        #region constructor

        private readonly BookShopDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ICurrentUser _currentUser;
        private readonly IDomainEventPublisher _domainEventPublisher; 
        public CrudRepository(BookShopDbContext dbContext, ICurrentUser currentUser,IDomainEventPublisher domainEventPublisher)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _domainEventPublisher = domainEventPublisher;
            _dbSet = dbContext.Set<TEntity>();   
        }


        #endregion




        public async Task Add(TEntity entity)
        {
            if(typeof(TKey) == typeof(Guid))
                entity.Id = (TKey)Convert.ChangeType(Guid.NewGuid(),typeof(TKey));

            DateTime dateTime = DateTime.UtcNow;
            entity.CreateDate = entity.LastModifiedDate = dateTime;
            entity.CreateBy = _currentUser.GetId();
            entity.LastModifiedBy = _currentUser.GetId(); 
            entity.LastModifiedDate = DateTime.UtcNow;
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            await entity.PublishAllDomainEventsAndClear(_domainEventPublisher);
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            await entity.PublishAllDomainEventsAndClear(_domainEventPublisher);

        }

        public async Task Delete(TKey key)
        {
            TEntity? entity = await _dbContext.Set<TEntity>().FindAsync(key);

            if (entity == null)
                throw new NotFoundException($"Entity with id ({key} not found)");
            
            await Delete(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .AsEnumerable();
        }

        public async Task<TEntity> Get(TKey key)
        {
            TEntity? entity = await _dbContext.Set<TEntity>()
                .FirstOrDefaultAsync(a => a.Id.Equals(key) && a.IsDeleted == false);

            if (entity == null)
                throw new NotFoundException($"Entity with id ({key} not found)");

            return entity;
        }

        public async Task Update(TEntity entity)
        {
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedBy = _currentUser.GetId();
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            await entity.PublishAllDomainEventsAndClear(_domainEventPublisher);
        }

        public async Task SoftDelete(TEntity entity)
        {
            entity.DeleteDate = DateTime.UtcNow;
            entity.DeletedBy = _currentUser.GetId();
            entity.IsDeleted = true;
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            await entity.PublishAllDomainEventsAndClear(_domainEventPublisher);
        }

        public async Task SoftDelete(TKey key)
        {
            TEntity? entity = await _dbContext.Set<TEntity>().FindAsync(key);
            
            if(entity == null)
                throw new NotFoundException($"Entity with id ({key}) not found)");

            await SoftDelete(entity);
        }

        public async void Dispose()
        {
            await _dbContext.DisposeAsync();
        }


        //public virtual Task<IEnumerable<TEntity>> GetAll<TQueryOption>(Action<TQueryOption> configQueryOption)
        //    where TQueryOption : IQueryOption<TEntity, TKey>, new()
        //{
        //    //TQueryOption queryOption = new TQueryOption();
        //    //configQueryOption.Invoke(queryOption);
        //    //var query = _dbSet.AsQueryable();
        //    //query = queryOption.ApplyOptions<IQueryable<TEntity>, IQueryable<TEntity>>(query);
        //    //return Task.FromResult(query.AsEnumerable());

        //    return GetAll();
        //}


        //public abstract Task<IEnumerable<TEntity>> GetAll<TQueryOption>(Action<TQueryOption> configQueryOption)
        //    where TQueryOption : IQueryOption<TEntity, TKey>, new();


        public virtual Task<TEntity> Get<TQueryOption>(TKey key, Action<TQueryOption> configQueryOption)
           where TQueryOption : IQueryOption<TEntity, TKey>, new()
        {
            //TQueryOption queryOption = new TQueryOption();
            //configQueryOption.Invoke(queryOption);
            //var query = _dbSet.AsQueryable();
            //query = queryOption.ApplyOptions<IQueryable<TEntity>, IQueryable<TEntity>>(query);
            //TEntity? entity = await query.FirstOrDefaultAsync(a => a.Id.Equals(key) && a.IsDeleted == false);
            //if (entity == null)
            //    throw new NotFoundException($"Entity with id ({key}) not found)");
            //return entity;

            return Get(key);
        }

        public async Task<TEntity> Get(string key)
        {
            string tbaleName = _dbSet.EntityType.GetTableName();
            TEntity? entity = await _dbSet.FromSqlRaw($"Select * From {tbaleName} Where Id = '{key}'").FirstOrDefaultAsync();
            if (entity == null)
                throw new NotFoundException($"Entity with id ({key}) not found)");
            return entity;
        }

       
    }



    internal abstract class CrudRepository<TEntity, TKey, TQueryOption> :
      CrudRepository<TEntity, TKey>,
      IDisposable,
      ICrudRepository<TEntity, TKey, TQueryOption>
      where TEntity : Entity<TKey>
      where TKey : struct
      where TQueryOption : IQueryOption<TEntity, TKey>, new()
    {
        
        protected readonly QueryOptionOperator<TEntity, TKey, TQueryOption> _queryOptionOperator = new();
        protected CrudRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher) : base(dbContext, currentUser, domainEventPublisher)
        {}


        public abstract Task<TEntity> Get(TKey key, Action<TQueryOption> queryOptionConfig);

        public abstract Task<IEnumerable<TEntity>> GetAll(Action<TQueryOption> configQueryOption);


    }

}

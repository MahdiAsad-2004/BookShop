using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.QueryOptions
{
    internal class QueryOptionOperator<TEntity, TKey, TQueryOption> : IQueryOption<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TQueryOption : IQueryOption<TEntity, TKey>, new()
    {
        public IQueryable<TEntity> PerformEntityIncludes(TQueryOption queryOption, IQueryable<TEntity> query)
        {
            var joinTableFields = queryOption.GetType()
                .GetFields()
                .Where(a => a.FieldType == typeof(IncludeEntity))
                .Select(a => a.GetValue(queryOption) as IncludeEntity);

            foreach (var joinTableField in joinTableFields.Where(a => a.Included))
            {
                query.Include(joinTableField!.EntityName);
            }

            return query;
        }

        public TQueryOption ConfigureQueryOption(Action<TQueryOption> queryOptionConfig)
        {
            TQueryOption queryOption = new();
            queryOptionConfig(queryOption);
            return queryOption;
        }

    }

}

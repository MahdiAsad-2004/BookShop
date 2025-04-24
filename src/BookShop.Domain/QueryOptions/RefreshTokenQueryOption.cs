using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;

namespace BookShop.Domain.QueryOptions
{
    public class RefreshTokenQueryOption : IQueryOption<RefreshToken,Guid>
    {
        public bool IncludeUser { get; init; }


    }
}

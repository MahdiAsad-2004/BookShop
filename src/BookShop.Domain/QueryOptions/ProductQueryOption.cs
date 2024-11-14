using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;

namespace BookShop.Domain.QueryOptions
{
    public class ProductQueryOption : IQueryOption<Product,Guid>
    {
        public bool IncludeReviews = false;

        public bool IncludeDiscounts = false;

        public int? StartPrice = null;
        
        public int? EndPrice = null;

        public bool ReviewsAccepted = false;

    }

}

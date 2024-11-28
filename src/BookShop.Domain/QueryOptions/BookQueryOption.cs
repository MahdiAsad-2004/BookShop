using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Domain.QueryOptions
{
    public class BookQueryOption : IQueryOption<Book,Guid>
    {
        public bool IncludeReviews = false;

        public bool IncludeDiscounts = false;

        public bool IncludeAuthors = false;
        
        public bool IncludeTranslator = false;

        public bool IncludePublisher = false;

        public int? StartPrice = null;
        
        public int? EndPrice = null;

        public bool? Available = null;
        
        public byte? AverageScore = null;
    }


    //public enum ProductSortingOrder
    //{
    //    Newest , Oldest,
    //    HighestPrice , LowestPrice,
    //    HighestDiscount , LowestDiscount,
    //    HighestSellCount , LowestSellCount,
    //    AlphabetDesc , AlphabetAsce
    //}

}

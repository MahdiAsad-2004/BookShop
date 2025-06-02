using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Domain.QueryOptions
{
    public class CartItemQueryOption : IQueryOption<CartItem,Guid>
    {
        public bool IncludeProduct { get; init; } = false;
        public bool IncludeDiscount { get; init; } = false;
        public Guid? UserId  { get; set; }
        public DateTime? CreateDate { get; init; } 
        public DateTime? ModifiedDate { get; init; }
        public int? MinQuantity { get; init; }
        public int? MaxQuantity { get; init; }
    }


    public enum CartItemSortingOrder
    {
        Newest, Oldest,
    }

}

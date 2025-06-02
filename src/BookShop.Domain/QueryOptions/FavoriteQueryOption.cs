using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Domain.QueryOptions
{
    public class FavoriteQueryOption : IQueryOption<Favorite,Guid>
    {
        public Guid? UserId  { get; set; }
        public Guid? ProductId  { get; set; }
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }
    }


    //public enum FavoriteSortingOrder
    //{
    //    Newest, Oldest,
    //}

}

using BookShop.Application.Common.Dtos;

namespace BookShop.Application.Features.CartItem.Dtos
{
    public class CartItemDetailDto : BaseDto
    {
        public Guid Product_Id { get; set; }
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public int? Product_DiscountedPrice { get; set; }
        public string Product_ImageName { get; set; }
        public int Quantity { get; set; }


    }
}

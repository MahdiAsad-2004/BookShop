using BookShop.Application.Features.CartItem.Dtos;

namespace BookShop.Application.Features.CartItem.Mapper
{
    internal static class CartItemMapper
    {
        public static CartItemDetailDto ToDetail(E.CartItem cartItem)
        {
            return new CartItemDetailDto
            {
                Id = cartItem.Id.ToString(),
                Product_Id = cartItem.ProductId,
                Product_Title = cartItem.Product.Title,
                Product_Price = cartItem.Product.Price,
                Product_DiscountedPrice = (int?)cartItem.Product.DiscountedPrice,
                Product_ImageName = cartItem.Product.ImageName,
                Quantity = cartItem.Quantity
            };
        }
        
        public static List<CartItemDetailDto> ToDetail(List<E.CartItem> cartItems)
        {
            return cartItems.Select(x => ToDetail(x)).ToList(); 
        }


    }
}

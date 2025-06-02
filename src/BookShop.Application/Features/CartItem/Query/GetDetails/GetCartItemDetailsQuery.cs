using BookShop.Application.Common.Request;
using BookShop.Application.Features.CartItem.Dtos;
using BookShop.Application.Features.CartItem.Mapper;
using BookShop.Domain.Entities;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;

namespace BookShop.Application.Features.CartItem.Query.GetDetails
{
    public class GetCartItemDetailsQuery : CachableRequest<List<CartItemDetailDto>>
    {
        public Guid? UserId { get; set; }

        public override TimeSpan CacheExpireTime => TimeSpan.FromMinutes(30);

        public override string GetCacheKey()
        {
            return $"cartItems-userId-{UserId}";
        }

    }


    internal class GetCartItemDetailsQueryHandler : IRequestHandler<GetCartItemDetailsQuery, List<CartItemDetailDto>>
    {
        #region constructor

        private readonly ICartItemRepository _cartItemRepository;
        public GetCartItemDetailsQueryHandler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        #endregion

        public async Task<List<CartItemDetailDto>> Handle(GetCartItemDetailsQuery request, CancellationToken cancellationToken)
        {
            //fetch
            List<E.CartItem> cartItems = await _cartItemRepository.GetAll(new CartItemQueryOption
            {
                IncludeProduct = true,
                IncludeDiscount = true,
                UserId = request.UserId,
            });

            //map
            List<CartItemDetailDto> cartItemDetailDtos = CartItemMapper.ToDetail(cartItems);

            return cartItemDetailDtos;
        }


    }


}

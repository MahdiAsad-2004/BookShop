using BookShop.Application.Caching;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.CartItem.Query.GetDetails;
using BookShop.Domain.Common;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using MediatR;

namespace BookShop.Application.Features.CartItem.Commands.Create
{
    public class CreateCartItemCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }



    internal class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand, Result<Empty>>
    {

        #region constructor

        private readonly ICache _cache;
        private readonly ICurrentUser _currentUser;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        public CreateCartItemCommandHandler(ICurrentUser currentUser, ICartRepository cartRepository, ICartItemRepository cartItemRepository, ICache cache)
        {
            _currentUser = currentUser;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _cache = cache;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.Id.Value;

            //remove cartItems cache
            GetCartItemDetailsQuery getCartItemDetailsQuery = new GetCartItemDetailsQuery()
            {
                UserId = userId,
            };
            _cache.Remove(getCartItemDetailsQuery.GetCacheKey());

            // create cart if does not have
            Guid? cartId = await _cartRepository.GetIdForUser(userId);
            if (cartId == null)
            {
                cartId = await _cartRepository.Create(new Cart
                {
                    UserId = userId,
                });
                await CreateCartItem(request, cartId.Value);
                return Result.Success();
            }

            Guid? cartItemId = await _cartItemRepository.GetId(request.ProductId, cartId.Value);
            cartItemId = await _cartItemRepository.GetId(request.ProductId, cartId.Value);
            if (cartItemId == null)
            {
                await CreateCartItem(request, cartId.Value);
                return Result.Success();
            }
            else
            {
                await _cartItemRepository.Update(cartItemId.Value, request.Quantity);
                return Result.Success();
            }
        }

        private async Task CreateCartItem(CreateCartItemCommand request, Guid cartId)
        {
            await _cartItemRepository.Create(new E.CartItem
            {
                CartId = cartId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
            });
        }


    }


}

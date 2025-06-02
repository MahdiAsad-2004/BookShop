using BookShop.Application.Common.Request;
using BookShop.Domain.Common;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.CartItem.Commands.Remove
{
    public class RemoveCartItemCommand : IRequest<Result<Empty>> , IValidatableRquest
    {
        public Guid CartItemId { get; set; }
    }



    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Result<Empty>>
    {
        #region constructor

        private readonly ICartItemRepository _cartItemRepository;
        public RemoveCartItemCommandHandler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        #endregion

        public async Task<Result<Empty>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            bool delete = false;
            delete = await _cartItemRepository.SoftDelete(request.CartItemId);
            if (delete)
            {
                return Result.Success("Product successfully removed from your cart.");
            }
            return new Result<Empty>(Empty.New, false, "Product was not exist in your cart.");
        }
    }
}

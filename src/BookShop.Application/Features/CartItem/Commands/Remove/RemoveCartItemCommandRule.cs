using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.CartItem.Commands.Remove
{
    public class RemoveCartItemCommandRule : BussinessRule<RemoveCartItemCommand>
    {
        #region constructor

        private readonly ICartItemRepository _cartItemRepository;
        public RemoveCartItemCommandRule(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        #endregion


        [RuleItem]
        public async Task CartItem_Must_Exist()
        {
            if (await _cartItemRepository.IsExist(_request.CartItemId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found , nameof(_request.CartItemId), $"Product was not exist in cart");
            }
        }


    }
}

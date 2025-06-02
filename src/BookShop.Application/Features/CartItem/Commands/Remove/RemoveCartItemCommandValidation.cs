using BookShop.Application.Common.Validation;
using FluentValidation;

namespace BookShop.Application.Features.CartItem.Commands.Remove
{
    public class RemoveFavoriteCommandValidation : AbstractValidator<RemoveCartItemCommand>
    {
        public RemoveFavoriteCommandValidation()
        {
            RuleFor(a => a.CartItemId)
                .NotNullOrEmpty();
        }

    }

}

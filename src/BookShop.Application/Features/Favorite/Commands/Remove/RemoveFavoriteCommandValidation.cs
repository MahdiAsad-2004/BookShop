using BookShop.Application.Common.Validation;
using FluentValidation;

namespace BookShop.Application.Features.Favorite.Commands.Remove
{
    public class RemoveFavoriteCommandValidation : AbstractValidator<RemoveFavoriteCommand>
    {
        public RemoveFavoriteCommandValidation()
        {
            RuleFor(a => a.FavoriteId)
                .NotNullOrEmpty();
        }

    }

}

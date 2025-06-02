using BookShop.Application.Common.Validation;
using FluentValidation;

namespace BookShop.Application.Features.Favorite.Commands.Create
{
    public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
    {
        public CreateFavoriteCommandValidator()
        {
            RuleFor(a => a.ProductId)
                .NotNullOrEmpty();
            
            RuleFor(a => a.UserId)
                .NotNullOrEmpty();
        
        
        
        }

    }

}

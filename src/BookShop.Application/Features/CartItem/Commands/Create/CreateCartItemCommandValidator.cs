using FluentValidation;

namespace BookShop.Application.Features.CartItem.Commands.Create
{
    public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
    {
        public CreateCartItemCommandValidator()
        {
            RuleFor(a => a.ProductId)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);


            RuleFor(a => a.Quantity)
                .NotNull()
                .GreaterThan(0);
           
        
        }


    }


}

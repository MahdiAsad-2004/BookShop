using FluentValidation;
using MediatR;

namespace BookShop.Application.Features.Book.Commands.Create
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(a => a.Title)
                .NotNull()
                .NotEmpty();
        }
    }

}

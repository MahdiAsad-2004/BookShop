using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Author.Commands.Create
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(a => a.ImageFile)
                .FileNotNull()
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);

            RuleFor(a => a.Gender)
                .NotNull()
                .IsInEnum();

        }

    }
}

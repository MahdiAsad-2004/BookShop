using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Author.Commands.Update
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(a => a.Name)
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(30);

            RuleFor(a => a.Gender)
                .NotNull()
                .IsInEnum();

            RuleFor(a => a.ImageFile)
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);
        }
    }




}

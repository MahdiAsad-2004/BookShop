using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Translator.Commands.Update
{
    public class UpdateTranslatorCommandValidator : AbstractValidator<UpdateTranslatorCommand>
    {
        public UpdateTranslatorCommandValidator()
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

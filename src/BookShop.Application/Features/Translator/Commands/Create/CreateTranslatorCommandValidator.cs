using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Translator.Commands.Create
{
    public class CreateTranslatorCommandValidator : AbstractValidator<CreateTranslatorCommand>
    {
        public CreateTranslatorCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(a => a.Gender)
                .NotNull()
                .IsInEnum();
            
            RuleFor(a => a.ImageFile)
                .FileNotNull()
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);

        }

    }
}

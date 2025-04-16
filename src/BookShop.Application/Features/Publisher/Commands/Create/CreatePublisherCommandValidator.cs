using BookShop.Application.Extensions;
using BookShop.Application.Features.Publisher.Commands.Create;
using BookShop.Application.Common.Validation;
using FluentValidation;

namespace BookShop.Application.Features.Category.Commands.Create
{
    public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherCommandValidator()
        {
            RuleFor(a => a.Title)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(a => a.ImageFile)
                .FileNotNull()
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);
        }

    }
}

using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.EBook.Commands.Update
{
    public class UpdateEBookCommandValidator : AbstractValidator<UpdateEBookCommand>
    {
        public UpdateEBookCommandValidator()
        {

            RuleFor(a => a.AuthorIds)
                  .NotEmpty();

            RuleFor(a => a.Edition)
                .GreaterThan(0);

            RuleFor(a => a.Language)
                .NotNull()
                .IsInEnum();

            RuleFor(a => a.NumberOfPages)
                .NotNull()
                .GreaterThan(0);

            RuleFor(a => a.Product_CategoryId)
                .Must(a => a != null && a == Guid.Empty ? false : true).WithMessage("{PropertyName} must not be empty");

            RuleFor(a => a.Product_DescriptionHtml)
                .NotNull()
                .MaximumLength(500);

            RuleFor(a => a.Product_ImageFile)
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);

            RuleFor(a => a.Product_NumberOfInventory)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(a => a.Product_Price)
                .NotNull()
                .GreaterThan(0);

            RuleFor(a => a.Product_Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(a => a.PublisherId)
                .NotNull();

            RuleFor(a => a.PublishYear)
                .NotNull()
                .LessThan(DateTime.Today);

            RuleFor(a => a.TranslatorId)
                .Must(a => a != null && a == Guid.Empty ? false : true).WithMessage("{PropertyName} must not be empty");

            RuleFor(a => a.EBookFile)
                .FileSizeMustLessThan(50)
                .FileExtensionMustBeIn(FileExtensions.EBookFileAllowedExtensions);

        }
    }




}

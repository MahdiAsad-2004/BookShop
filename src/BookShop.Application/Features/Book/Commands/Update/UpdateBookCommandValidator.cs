using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Book.Commands.Update
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {

            RuleFor(a => a.AuthorIds)
                .NotEmpty();

            RuleFor(a => a.Cover)
                .NotNull()
                .IsInEnum();

            RuleFor(a => a.Cutting)
                .NotNull()
                .IsInEnum();

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
                .Must(a => a == null || (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                .Must(a => a == null || FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .WithMessage("Image file extension is not allowed");

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

            RuleFor(a => a.WeightInGram)
                .GreaterThan(0)
                .LessThan(10_000);



        }
    }




}

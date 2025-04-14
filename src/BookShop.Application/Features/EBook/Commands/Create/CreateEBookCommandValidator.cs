using BookShop.Application.Extensions;
using BookShop.Application.Features.EBook.Commands.Create;
using FluentValidation;
using MediatR;

namespace BookShop.Application.Features.EBook.Commands.Create
{
    public class CreateEBookCommandValidator : AbstractValidator<CreateEBookCommand>
    {
        public CreateEBookCommandValidator()
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
                .NotNull()
                .Must(a => a != null && a is not null).WithMessage("{PropertyName} must not be null")
                    .Must(a => (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                        .When(req => req.Product_ImageFile != null)
                    .Must(a => FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0,1) , StringComparison.OrdinalIgnoreCase)))
                        .When(req => req.Product_ImageFile != null)
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

            RuleFor(a => a.EBookFile)
                .NotNull()
                .Must(a => a != null && a is not null).WithMessage("{PropertyName} must not be null")
                    .Must(a => (float)(a.Length / 1024f / 1000f) <= 50.0f).WithMessage("Image size must be less than 50MB")
                        .When(req => req.EBookFile != null)
                    .Must(a => FileExtensions.EBookFileAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                        .When(req => req.EBookFile != null)
                            .WithMessage("EBook file extension is not allowed");



        }
    }

}

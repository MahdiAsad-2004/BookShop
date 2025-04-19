using BookShop.Application.Common.Validation;
using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Category.Commands.Update
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(a => a.Title)
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(30);

            RuleFor(a => a.ParentId)
                .Must(a => a == null || a != Guid.Empty).WithMessage("Parent Id is empty");

            RuleFor(a => a.ImageFile)
                .FileSizeMustLessThan(3)
                .FileExtensionMustBeIn(FileExtensions.ImageAllowedExtensions);
        }
    }




}

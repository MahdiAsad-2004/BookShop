using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Category.Commands.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(a => a.Title)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            
            RuleFor(a => a.ParentId)
                .Must(a => a == null || a != Guid.Empty).WithMessage("Parent Id is empty");    
            

            RuleFor(a => a.ImageFile)
                .NotNull()
                .Must(a => (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                .Must(a => FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .WithMessage("Image file extension is not allowed");

        }

    }
}

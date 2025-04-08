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
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(a => a.ImageFile)
                .Must(a => a == null || (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                .Must(a => a == null || FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .WithMessage("Image file extension is not allowed");

            RuleFor(a => a.Gender)
                .NotNull()
                .IsInEnum();

        }

    }
}

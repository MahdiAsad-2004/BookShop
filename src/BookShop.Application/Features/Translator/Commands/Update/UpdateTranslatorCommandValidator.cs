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
                .Must(a => a == null || (float)(a.Length / 1024f / 1000f) <= 3.0f)
                    .When(req => req.ImageFile != null)
                        .WithMessage("Image size must be less than 3MB")
                .Must(a => a == null || FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .When(req => req.ImageFile != null)
                        .WithMessage("Image file extension is not allowed");
        }
    }




}

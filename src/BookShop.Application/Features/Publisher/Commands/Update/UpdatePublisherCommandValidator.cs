using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Publisher.Commands.Update
{
    public class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
    {
        public UpdatePublisherCommandValidator()
        {
            RuleFor(a => a.Title)
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(30);

            RuleFor(a => a.ImageFile)
                .Must(a => (float)(a.Length / 1024f / 1000f) <= 3.0f)
                    .When(req => req.ImageFile != null)
                        .WithMessage("Image size must be less than 3MB")
                .Must(a => FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .When(req => req.ImageFile != null)
                        .WithMessage("Image file extension is not allowed");

        }
    }




}

﻿using BookShop.Application.Extensions;
using FluentValidation;

namespace BookShop.Application.Features.Discount.Commands.Create
{
    public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(a => a.DiscountPercent)
                .Must(a => a == null || (a.Value > 0 && a.Value < 100f)).WithMessage("{PropertyName} must between 0 and 100")
                .Must((a, b) => a.DiscountPercent == null && a.DiscountPrice == null ? false : true).WithMessage("Discount must have either percent or perice")
                .Must((a, b) => a.DiscountPercent != null && a.DiscountPrice != null ? false : true).WithMessage("Discount can not be with both percent and price");
            
            RuleFor(a => a.DiscountPrice)
                .Must(a => a == null || a.Value > 0).WithMessage("{PropertyName} must greater than 0")
                .Must((a,b) => a.DiscountPercent == null && a.DiscountPrice == null ? false : true).WithMessage("Discount must have either percent or perice")
                .Must((a,b) => a.DiscountPercent != null && a.DiscountPrice != null ? false : true).WithMessage("Discount can not be with both percent and price");

            RuleFor(a => a.EndDate)
                .Must(a => a == null || a.Value > DateTime.UtcNow).WithMessage("{PropertyName} must be after now")
                .Must((a,b) => a.EndDate != null && a.StartDate != null ? a.EndDate.Value > a.StartDate.Value : true)
                    .WithMessage("{PropertyName} must be after StartDate");

            RuleFor(a => a.MaximumUseCount)
                .Must(a => a != null ? a.Value > 0 : true).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(a => a.Name)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(a => a.Priority)
                .NotNull()
                .GreaterThan(0);

            RuleFor(a => a.StartDate)
                .Must(a => a == null || a.Value > DateTime.UtcNow).WithMessage("{PropertyName} must be after now")
                .Must((a,b) => a.EndDate != null && a.StartDate != null ? a.EndDate.Value > a.StartDate.Value : true)
                    .WithMessage("{PropertyName} must be before EndDate");


        }

    }
}

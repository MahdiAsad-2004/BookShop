
using BookShop.Application.Extensions;
using BookShop.Application.Features.Category.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Category.Commands.Update
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(a => a.Title)
               .NotNull()
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(30);

            RuleFor(a => a.ParentId)
                .Must(a => a == null || a != Guid.Empty).WithMessage("Parent Id is empty");

            RuleFor(a => a.ImageFile)
                .Must(a => a == null || (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                .Must(a => a == null || FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .WithMessage("Image file extension is not allowed");
        }
    }




}

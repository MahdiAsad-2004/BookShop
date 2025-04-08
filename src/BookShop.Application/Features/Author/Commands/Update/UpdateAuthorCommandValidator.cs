
using BookShop.Application.Extensions;
using BookShop.Application.Features.Author.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Author.Commands.Update
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(a => a.Name)
               .NotNull()
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(30);

            RuleFor(a => a.Gender)
                .NotNull()
                .IsInEnum();

            RuleFor(a => a.ImageFile)
                .Must(a => a == null || (float)(a.Length / 1024f / 1000f) <= 3.0f).WithMessage("Image size must be less than 3MB")
                .Must(a => a == null || FileExtensions.ImageAllowedExtensions.Any(b => b.Equals(Path.GetExtension(a.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)))
                    .WithMessage("Image file extension is not allowed");
        }
    }




}

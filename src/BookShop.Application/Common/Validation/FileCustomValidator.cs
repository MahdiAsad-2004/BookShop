using BookShop.Application.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace BookShop.Application.Common.Validation
{
    public static class FileCustomValidators
    {
        public static IRuleBuilderOptions<T, IFormFile> FileSizeMustLessThan<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, float size_Mb)
        {
            return ruleBuilder.Must((rootObject, file, context) =>
            {
                context.MessageFormatter.AppendArgument("maxSize", size_Mb);
                if (file == null)
                    return true;
                return (float)(file.Length / 1024f / 1000f) <= size_Mb;
            })
            .WithMessage("{PropertyName} size must be less than {maxSize}MB ");
        }

        public static IRuleBuilderOptions<T, IFormFile> FileExtensionMustBeIn<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, string[] allowedExtensions)
        {
            return ruleBuilder.Must((rootObject, file, context) =>
            {
                if (file == null)
                    return true;

                string fileExtention = Path.GetExtension(file.FileName).Remove(0, 1);
                return allowedExtensions.Any(b => b.Equals(fileExtention, StringComparison.OrdinalIgnoreCase));
            })
            .WithMessage("{PropertyName} file extension is not allowed ");
        }

        public static IRuleBuilderOptions<T, IFormFile> FileNotNull<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, file, context) =>
            {
                return file != null;
            })
            .WithMessage("{PropertyName} must not be null");

        }


        public static IRuleBuilderOptions<T, IFormFile?> FileNotNulll<T>(this IRuleBuilder<T, IFormFile?> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, file, context) =>
            {
                return file != null;
            })
            .WithMessage("{PropertyName} must not be null");
        }

    }
}

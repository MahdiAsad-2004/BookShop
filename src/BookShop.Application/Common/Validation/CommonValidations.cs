
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Common.Validation
{
    internal static class CommonValidations
    {
        public static IRuleBuilderOptions<T, Guid> NotNullOrEmpty<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, value, context) =>
            {
                if (value == null || value == Guid.Empty)
                    return false;

                return true;
            })
            .WithMessage("{PropertyName} must not be empty");
        }

        public static IRuleBuilderOptions<T, Guid?> NotNullOrEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, value, context) =>
            {
                if (value == null || value == Guid.Empty)
                    return false;

                return true;
            })
            .WithMessage("{PropertyName} must not be null or empty");
        }

    }
}

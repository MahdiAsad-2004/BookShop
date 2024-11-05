
using BookShop.Domain.Exceptions;
using FluentValidation.Results;

namespace BookShop.Application.Extensions
{
    public static class ValidationErrorExtension
    {
        public static ValidationError ToValidationError(this ValidationFailure validationFailure)
        {
            return new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage);
        }

        public static List<ValidationError> ToValidationErrors(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .Select(validationFailure => validationFailure.ToValidationError())
                .ToList();
        }
    }
}

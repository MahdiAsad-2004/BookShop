using BookShop.Domain.Common;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using FluentValidation.Results;

namespace BookShop.Application.Extensions
{
    public static class ErrorDetailExtension
    {
        //public static ValidationError ToValidationError(this ValidationFailure validationFailure)
        //{
        //    return new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage);
        //}

        //public static List<ValidationError> ToValidationErrors(this IEnumerable<ValidationFailure> validationFailures)
        //{
        //    return validationFailures
        //        .Select(validationFailure => validationFailure.ToValidationError())
        //        .ToList();
        //}

        public static ErrorDetail ToErrorDetail(this ValidationFailure validationFailure)
        {
            ErrorCode errorCode = Enum.Parse<ErrorCode>(validationFailure.ErrorCode);
            return new ErrorDetail(errorCode,validationFailure.PropertyName, validationFailure.ErrorMessage);
        }

        public static List<ErrorDetail> ToErrorDetails(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .Select(validationFailure => ToErrorDetail(validationFailure))
                .ToList();
        }

    }
}

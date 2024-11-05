using BookShop.Application.Extensions;
using FluentValidation;
using MediatR.Pipeline;
using BookShop.Domain.Exceptions;

namespace BookShop.Application.Behaviours.PreProcessors
{
    internal class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {

        private readonly IValidator<TRequest> _validator;
        public ValidationPreProcessor(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var failures = await _validator.ValidateAsync(request, cancellationToken);

            if (failures.IsValid == false)
                throw new Domain.Exceptions.ValidationException(failures.Errors.ToValidationErrors());
        }

    }
}

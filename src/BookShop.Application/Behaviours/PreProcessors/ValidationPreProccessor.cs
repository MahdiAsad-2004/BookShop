using BookShop.Application.Extensions;
using FluentValidation;
using MediatR.Pipeline;
using BookShop.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Application.Behaviours.PreProcessors
{
    internal class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {

        private readonly IValidator<TRequest>? _validator;
        private readonly IServiceProvider _serviceProvider;
        public ValidationPreProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _validator = _serviceProvider.GetService<IValidator<TRequest>>();
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                var failures = await _validator.ValidateAsync(request, cancellationToken);

                if (failures.IsValid == false)
                    throw new Domain.Exceptions.ValidationException(failures.Errors.ToValidationErrors());
            }
        }

    }
}

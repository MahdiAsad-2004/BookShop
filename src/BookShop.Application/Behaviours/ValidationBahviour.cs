using BookShop.Application.Common.Request;
using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Extensions;
using BookShop.Domain.Common;
using BookShop.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace BookShop.Application.Behaviours
{
    public class ValidationBahviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IValidatableRquest
        where TResponse : Result, new()
    {

        #region constructor

        private readonly IValidator<TRequest>? _validator;
        private readonly BussinessRule<TRequest>? _bussinessRule;
        private readonly ILogger _logger;
        public ValidationBahviour(ILogger logger,IServiceProvider serviceProvider)
        {
            _logger = logger;
            _bussinessRule = serviceProvider.GetService<BussinessRule<TRequest>>();
            _validator = serviceProvider.GetService<IValidator<TRequest>>();
        }

        #endregion


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information($"Validating {nameof(request)} request.");

            if (_validator != null)
            {
                var failures = await _validator.ValidateAsync(request, cancellationToken);
                if (failures.IsValid == false)
                {
                    return new TResponse()
                    {
                        ResultData = null,
                        IsSuccess = false,
                        Error = new Error(ErrorCode.Validation_Failed, $"{failures.Errors.Count} validation error occured", failures.Errors.ToErrorDetails())
                    };
                }
            }
            if (_bussinessRule != null)
            {
                _bussinessRule.Confing(request,true);

                var ruleMethods = _bussinessRule.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(a => a.GetCustomAttribute<RuleItemAttribute>() != null).ToList();

                foreach (var ruleMethod in ruleMethods) 
                {
                    Task task = (Task)ruleMethod.Invoke(_bussinessRule , null)!;
                    await task;

                    if (_bussinessRule.Stop())
                        break;
                }   
                
                if (_bussinessRule.ErrorDetails.Any())
                {
                    ErrorCode mainErrorCode = ErrorCode.Validation_Failed;
                    if(_bussinessRule.ErrorDetails.Count == 1)
                    {
                        mainErrorCode = _bussinessRule.ErrorDetails[0].Code;
                    }
                    return new TResponse()
                    {
                        ResultData = null,
                        IsSuccess = false,
                        Error = new Error(mainErrorCode, "Request has error", _bussinessRule.ErrorDetails)
                    };
                }
            }

            _logger.Information($"{nameof(request)} request is validated");

            return await next();
        }
    }
}

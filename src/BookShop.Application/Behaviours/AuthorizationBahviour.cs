﻿using BookShop.Application.Authorization;
using BookShop.Application.Caching;
using BookShop.Application.Common.Request;
using BookShop.Domain.Exceptions;
using MediatR;
using Serilog;
using System.Reflection;

namespace BookShop.Application.Behaviours
{
    public class AuthorizationBahviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICashableRequest
        where TResponse : class
    {

        #region constructor

        private readonly ILogger _logger;
        private readonly IPermissionChecker _permissionChecker;
        public AuthorizationBahviour(ILogger logger, IPermissionChecker permissionChecker)
        {
            _logger = logger;
            _permissionChecker = permissionChecker;
        }

        #endregion


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information($"Authorizing {nameof(request)} request.");

            string[]? requiredPermissionNames = typeof(TRequest).GetCustomAttribute<RequiredPermission>()?.GetRequiredPermissions();

            if (requiredPermissionNames != null)
                if (await _permissionChecker.HasPermission(requiredPermissionNames))
                {
                    _logger.Information($"{nameof(request)} request is unauthorized (for required permissions).");
                    throw new UnauthorizeException("You does not have required permission for this operation");
                }

            _logger.Information($"{nameof(request)} request is authorized");

            return await next();
        }
    }
}
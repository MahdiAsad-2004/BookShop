using BookShop.Domain.Identity;
using MediatR.Pipeline;
using Serilog;

namespace BookShop.Application.Behaviours.PreProcessors
{
    public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        #region constructor

        private readonly ILogger _logger;
        private readonly ICurrentUser _currentUser;
        public LoggingPreProcessor(ILogger logger, ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        #endregion


        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = nameof(TRequest);
            var userName = _currentUser.Name;
            var userId = _currentUser.Id;
            _logger.Information($"Request: {requestName} with {request} by {userName}({userId})");
            return Task.CompletedTask;
        }
    }
}

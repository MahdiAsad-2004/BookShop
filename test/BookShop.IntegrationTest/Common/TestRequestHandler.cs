using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Common
{
    public class TestRequestHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TestRequestHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task SendRequest<TRequest>(TRequest request) where TRequest : MediatR.IRequest
        {
            using (var scopr = _serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(request);
            }
        }

        public async Task<TRessponse> SendRequest<TRequest, TRessponse>(TRequest request) where TRequest : IRequest<TRessponse>
        {
            using (var scopr = _serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>();
                TRessponse ressponse = await mediator.Send(request);
                return ressponse;
            }
        }



    }
}

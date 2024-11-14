

using FluentValidation;
using MediatR;

namespace BookShop.Application.Features.Product.Queries
{
    public class GetProductQuery : IRequest<Domain.Entities.Product>
    {

    }



    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Domain.Entities.Product>
    {
        public async Task<Domain.Entities.Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Title = "testProduct",
            };
        }
    }



    //public class GetProducttValidator : AbstractValidator<GetProductQuery>
    //{
    //    public GetProducttValidator()
    //    {
            
    //    }
    //}


}

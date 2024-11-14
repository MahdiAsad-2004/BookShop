using AutoMapper;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;

namespace BookShop.Application.Features.Product.Queries.GetSummary
{
    public class GetProductSummaryQuery : IRequest<ProductSummaryDto>
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
    }


    public class GetProductSummaryQueryHandler : IRequestHandler<GetProductSummaryQuery, ProductSummaryDto>
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductSummaryQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        #endregion


        public async Task<ProductSummaryDto> Handle(GetProductSummaryQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = null;
            if(request.Id != null)
            {
                product = await _productRepository.Get(request.Id.Value,new ProductQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeReviews = true,
                });
            }
            else if(string.IsNullOrWhiteSpace(request.Title) == false)
            {
                product = await _productRepository.GetByTitle(request.Title , new ProductQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeReviews = true,
                });
            }
            else 
            {   
            }

           
            return _mapper.Map<ProductSummaryDto>(product); 
        }

    }



}

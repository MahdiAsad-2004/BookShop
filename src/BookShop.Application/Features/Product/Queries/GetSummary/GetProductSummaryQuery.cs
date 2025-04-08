using AutoMapper;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Product.Queries.GetSummary
{
    public class GetProductSummaryQuery : CachableRequest<ProductSummaryDto>
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public ProductType? ProductType { get; set; } = null;
        public override TimeSpan CacheExpireTime => TimeSpan.FromMinutes(10);
        public override string GetCacheKey()
        {
            if (string.IsNullOrEmpty(_CacheKey))
                _CacheKey = $"{nameof(GetProductSummaryQuery)}-{Id}";
            return _CacheKey;
        }
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
            if (request.Id != null)
            {
                product = await _productRepository.GetWithQuery(request.Id.Value, new ProductQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeReviews = true,
                });
            }
            else if (string.IsNullOrWhiteSpace(request.Title) == false)
            {
                product = await _productRepository.GetByTitle(request.Title, new ProductQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeReviews = true,
                });
            }
            else
            {
                throw new NotFoundException("Request is not valid!");
            }


            return _mapper.Map<ProductSummaryDto>(product);
        }

    }



}

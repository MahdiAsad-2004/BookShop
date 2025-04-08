using AutoMapper;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Product.Queries.GetSummaries
{
    public class GetProductSummariesQuery : CachableRequest<PaginatedDtos<ProductSummaryDto>>
    {
        public ProductSortingOrder? SortingOrder { get; init; }
        public Paging? Paging { get; init; }
        public int? StartPrice { get; init; }
        public int? EndPrice { get; init; }
        public string? Title { get; init; }
        public bool? Available { get; init; }
        public byte? AverageScore { get; init; }
        public ProductType? ProductType { get; init; }
        public override TimeSpan CacheExpireTime => TimeSpan.FromMinutes(30);
        public override string GetCacheKey()
        {
            if (string.IsNullOrEmpty(_CacheKey))
                _CacheKey = RequestCacheKey.GetKey<GetProductSummariesQuery, PaginatedDtos<ProductSummaryDto>>(this);
            return _CacheKey;
        }
    }


    internal class GetProductSummariesQueryHandler : IRequestHandler<GetProductSummariesQuery, PaginatedDtos<ProductSummaryDto>>
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductSummariesQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task<PaginatedDtos<ProductSummaryDto>> Handle(GetProductSummariesQuery request, CancellationToken cancellationToken)
        {
            PaginatedEntities<Domain.Entities.Product> paginatedProducts = await _productRepository.GetAllWithQuery(
                new ProductQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeReviews = true,
                    Title = request.Title,
                    StartPrice = request.StartPrice,
                    EndPrice = request.EndPrice,
                    ProductType = request.ProductType,
                    Available = request.Available,
                    AverageScore = request.AverageScore,
                },
                sortingOrder: request.SortingOrder,
                paging: request.Paging);

            var productSummaryDtos = _mapper.Map<List<ProductSummaryDto>>(paginatedProducts.Entites.ToList());

            return new PaginatedDtos<ProductSummaryDto>(productSummaryDtos, paginatedProducts.Paging, paginatedProducts.TotalItemCount);
        }

    }



}

using AutoMapper;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Product.Queries.GetSummaries
{
    public class GetProductSummariesQuery : IRequest<PaginatedDtos<ProductSummaryDto>>
    {
        public ProductSortingOrder? SortingOrder { get; set; } = null;
        public Paging? Paging { get; set; } = null;
        public int? StartPrice { get; set; }
        public int? EndPrice { get; set; }
        public string? Title { get; set; }
        public bool? Available { get; set; }
        public byte? AverageScore { get; set; }
        public ProductType? ProductType { get; set; }
        
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
                    StartPrice = request.StartPrice,
                    EndPrice = request.EndPrice,
                    ProductType = request.ProductType,
                    Available = request.Available,
                    AverageScore = request.AverageScore,
                },
                sortingOrder: request.SortingOrder,
                paging: request.Paging);

            var productSummaryDtos = _mapper.Map<List<ProductSummaryDto>>(paginatedProducts.Entites.ToList());

            return new PaginatedDtos<ProductSummaryDto>(productSummaryDtos , paginatedProducts.Paging , paginatedProducts.TotalItemCount);
        }

    }



}

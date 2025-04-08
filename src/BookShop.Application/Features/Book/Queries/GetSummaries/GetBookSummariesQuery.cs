using AutoMapper;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Book.Queries.GetSummaries
{
    public class GetBookSummariesQuery : CachableRequest<PaginatedDtos<BookSummaryDto>>
    {
        public BookSortingOrder? SortingOrder { get; init; }
        public Paging? Paging { get; init; } 
        public int? StartPrice { get; init; }
        public int? EndPrice { get; init; }
        public string? Title { get; init; }
        public bool? IsAvailable { get; init; }
        public byte? AverageScore { get; init; }
        public Cover? Cover { get; init; }
        public Cutting? Cutting { get; init; }
        public Language? Language { get; init; }
        public DateTime? StartPublishYear { get; init; }
        public DateTime? EndPublishYear { get; init; }
        public Guid? PublisherId { get; init; }
        public Guid? TranslatorId { get; init; }
        public Guid? CategoryId { get; init; }
        public Guid? AuthorId { get; init; }
        public override TimeSpan CacheExpireTime => TimeSpan.FromMinutes(30);
        public override string GetCacheKey()
        {
            if (string.IsNullOrEmpty(_CacheKey))
                _CacheKey = RequestCacheKey.GetKey<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(this);
            return _CacheKey;
        }

        //public string CacheKey => RequestCacheKey.GetKey<GetBookSummariesQuery,PaginatedDtos<BookSummaryDto>>(this);

    }


    internal class GetBookSummariesQueryHandler : IRequestHandler<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>
    {
        #region constructor

        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetBookSummariesQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task<PaginatedDtos<BookSummaryDto>> Handle(GetBookSummariesQuery request, CancellationToken cancellationToken)
        {
            PaginatedEntities<Domain.Entities.Book> paginatedBooks = await _bookRepository.GetAll(
                new BookQueryOption
                {
                    IncludeDiscounts = true,
                    IncludeProduct = true,
                    IncludeReviews = true,
                    Product_Title = request.Title,
                    Product_IsAvailable = request.IsAvailable,
                    Product_AverageScore = request.AverageScore,
                    Product_EndPrice = request.EndPrice,
                    Product_StartPrice = request.StartPrice,
                    StartPublishYear = request.StartPublishYear,
                    EndPublishYear = request.EndPublishYear,
                    CategoryId = request.CategoryId,
                    Cover = request.Cover,
                    Cutting = request.Cutting,
                    Language = request.Language,
                    PublisherId = request.PublisherId,
                    TranslatorId = request.TranslatorId,
                    AuthorId = request.AuthorId,
                },
            request.Paging,
            request.SortingOrder
            );

            var bookSummaryDtos = _mapper.Map<List<BookSummaryDto>>(paginatedBooks.Entites.ToList());

            return new PaginatedDtos<BookSummaryDto>(bookSummaryDtos , paginatedBooks.Paging , paginatedBooks.TotalItemCount);
        }
    }

}

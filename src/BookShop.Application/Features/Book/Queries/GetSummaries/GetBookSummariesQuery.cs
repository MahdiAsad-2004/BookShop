using AutoMapper;
using BookShop.Application.Common.Dtos;
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
    public class GetBookSummariesQuery : IRequest<PaginatedDtos<BookSummaryDto>>
    {
        public BookSortingOrder? SortingOrder { get; set; } = null;
        public Paging? Paging { get; set; } = null;
        public int? StartPrice { get; set; }
        public int? EndPrice { get; set; }
        public string? Title { get; set; }
        public bool? Available { get; set; }
        public byte? AverageScore { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Languages Language { get; set; }
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }
        public DateTime? StartPublishYear { get; set; }
        public DateTime? EndPublishYear { get; set; }
        public Guid? CategoryId { get; set; }

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
                    Product_Available = request.Available,
                    Product_AverageScore = request.AverageScore,
                    Product_EndPrice = request.EndPrice,
                    Product_StartPrice = request.StartPrice,
                    StartPublishYear = request.StartPublishYear,
                    EndPublishYear = request.EndPublishYear,
                    CategoryId = request.CategoryId,
                },
            request.Paging,
            request.SortingOrder
            );

            var bookSummaryDtos = _mapper.Map<List<BookSummaryDto>>(paginatedBooks.Entites.ToList());

            return new PaginatedDtos<BookSummaryDto>(bookSummaryDtos , paginatedBooks.Paging , paginatedBooks.TotalItemCount);
        }
    }

}

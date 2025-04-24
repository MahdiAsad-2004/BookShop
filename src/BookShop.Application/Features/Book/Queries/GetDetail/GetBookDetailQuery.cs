using AutoMapper;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Book.Queries.GetDetail
{
    public class GetBookDetailQuery : CachableRequest<BookDetailDto>
    {
        public Guid Id { get; init; }
        public override TimeSpan CacheExpireTime { get; } = TimeSpan.FromMinutes(10);
        public override string GetCacheKey()
        {
            if (string.IsNullOrEmpty(_CacheKey))
                _CacheKey = $"{nameof(GetBookDetailQuery)}-{Id}";
            return _CacheKey;
        }
    }

    internal class GetBookDetailQueryHandler : IRequestHandler<GetBookDetailQuery, BookDetailDto>
    {
        #region constructor

        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetBookDetailQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task<BookDetailDto> Handle(GetBookDetailQuery request, CancellationToken cancellationToken)
        {
            E.Book book = await _bookRepository.Get(request.Id, new BookQueryOption
            {
                IncludeProduct = true,
                IncludeReviews = true,
                IncludeDiscounts = true,
                IncludeAuthors = true,
                IncludePublisher = true,
                IncludeTranslator = true,
            });

            BookDetailDto bookDetailDto = _mapper.Map<BookDetailDto>(book);

            return bookDetailDto;
        }
    }


}

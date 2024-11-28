using AutoMapper;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;

namespace BookShop.Application.Features.Book.Queries.GetDetail
{
    public class GetBookDetailQuery : IRequest<BookDetailDto>
    {
        public Guid Id { get; init; }
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
            Domain.Entities.Book book = await _bookRepository.Get(request.Id, new BookQueryOption
            {
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

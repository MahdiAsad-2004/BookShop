using BookShop.Application.Features.Favorite.Mapper;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;

namespace BookShop.Application.Features.Favorite.Queries.GetAll
{
    public class GetAllFavoritesQuery : IRequest<List<GetAllFavoritesQueryResponse>>
    {
        public Guid? UserId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }
    }


    internal class GetAllFavoritesQueryHandler : IRequestHandler<GetAllFavoritesQuery, List<GetAllFavoritesQueryResponse>>
    {
        #region constructor

        private readonly IFavoriteRepository _favoriteRepository;
        public GetAllFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        #endregion

        public async Task<List<GetAllFavoritesQueryResponse>> Handle(GetAllFavoritesQuery request, CancellationToken cancellationToken)
        {
            //fetch entities
            E.Favorite[] favorites = await _favoriteRepository.GetAll(new FavoriteQueryOption
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                FromCreateDate = request.FromCreateDate,
                ToCreateDate = request.ToCreateDate,
            });

            //mapping
            List<GetAllFavoritesQueryResponse> response = FavoriteMapper.ToQueryResponse(favorites);

            return response;
        }








    }



}

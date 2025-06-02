
using BookShop.Application.Features.Favorite.Queries.GetAll;

namespace BookShop.Application.Features.Favorite.Mapper
{
    internal static class FavoriteMapper
    {

        public static GetAllFavoritesQueryResponse ToQueryResponse(E.Favorite favorite)
        {
            return new GetAllFavoritesQueryResponse(favorite.Id, favorite.UserId, favorite.ProductId, favorite.CreateDate);
        }

        
        public static List<GetAllFavoritesQueryResponse> ToQueryResponse(E.Favorite[] favorites)
        {
            return favorites.Select(a =>  ToQueryResponse(a)).ToList(); 
        }



    }
}

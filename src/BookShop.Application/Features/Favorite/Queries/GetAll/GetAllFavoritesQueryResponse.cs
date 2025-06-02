
namespace BookShop.Application.Features.Favorite.Queries.GetAll
{
    internal record GetAllFavoritesQueryResponse(Guid FavoriteId, Guid UserId, Guid ProductId,DateTime CreateDate);


}

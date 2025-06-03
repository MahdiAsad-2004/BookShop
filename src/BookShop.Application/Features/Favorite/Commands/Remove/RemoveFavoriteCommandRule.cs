using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Favorite.Commands.Remove
{
    public class RemoveFavoriteCommandRule : BussinessRule<RemoveFavoriteCommand>
    {
        #region constructor

        private readonly IFavoriteRepository _favoriteRepository;
        public RemoveFavoriteCommandRule(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        #endregion



        [RuleItem]
        public async Task FavoriteId_Must_Exist()
        {
            if(await _favoriteRepository.IsExist(_request.FavoriteId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found,nameof(_request.FavoriteId), "User favorite not found");
            }
        }




    }

}

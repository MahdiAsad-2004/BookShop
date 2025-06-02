using BookShop.Application.Common.Request;
using BookShop.Domain.Common;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Favorite.Commands.Remove
{
    public class RemoveFavoriteCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid FavoriteId { get; set; }
    }


    internal class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, Result<Empty>>
    {
        #region constructor

        private readonly IFavoriteRepository _favoriteRepository;
        public RemoveFavoriteCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        #endregion

        public async Task<Result<Empty>> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            bool deleted = await _favoriteRepository.SoftDelete(request.FavoriteId);
            
            if(deleted == false)
                return Result.Fail("Removing an user favorite failed");
            
            return Result.Success("User favorite successfully removed");
        }
    }


}

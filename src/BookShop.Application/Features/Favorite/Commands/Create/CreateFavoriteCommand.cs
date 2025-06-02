using BookShop.Application.Common.Request;
using BookShop.Domain.Common;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Favorite.Commands.Create
{
    public class CreateFavoriteCommand : IRequest<Result<Empty>>,IValidatableRquest
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }


    public class CreateFavoriteCommandHandler : IRequestHandler<CreateFavoriteCommand, Result<Empty>>
    {
        #region constructor

        private readonly IFavoriteRepository _favoriteRepository;
        public CreateFavoriteCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        #endregion

        public async Task<Result<Empty>> Handle(CreateFavoriteCommand request, CancellationToken cancellationToken)
        {
            if(await _favoriteRepository.IsExist(request.UserId , request.ProductId))
            {
                return Result.Fail("Product already exist in user favorites.");
            }

            E.Favorite favorite = new E.Favorite
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
            };

            await _favoriteRepository.Add(favorite);

            return Result.Success("Product successfully added to user favorites.");
        }

    }

}

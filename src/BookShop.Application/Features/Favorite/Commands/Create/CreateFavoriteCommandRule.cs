using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Favorite.Commands.Create
{
    public class CreateFavoriteCommandRule : BussinessRule<CreateFavoriteCommand>
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        public CreateFavoriteCommandRule(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        #endregion



        [RuleItem]
        public async Task ProductId_Must_Exist()
        {
            if(await _productRepository.IsExist(_request.ProductId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found, nameof(_request.ProductId), "Product not found");
            }
        }

        
        [RuleItem]
        public async Task UserId_Must_Exist()
        {
            if(await _userRepository.IsExist(_request.UserId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found,nameof(_request.UserId), "User not found");
            }
        }




    }

}

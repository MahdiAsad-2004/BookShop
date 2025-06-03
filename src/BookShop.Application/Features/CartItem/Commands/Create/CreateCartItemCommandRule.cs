using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Application.Features.CartItem.Commands.Create
{
    public class CreateCartItemCommandRule : BussinessRule<CreateCartItemCommand>
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        public CreateCartItemCommandRule(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion



        [RuleItem]
        public async Task ProductId_Must_Exist()
        {
            if(await _productRepository.IsExist(_request.ProductId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found, nameof(_request.ProductId), $"Product not found");
            }
        }






    }


}

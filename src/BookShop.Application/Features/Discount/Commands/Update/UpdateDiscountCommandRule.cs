using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Discount.Commands.Create;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Discount.Commands.Update
{
    public class UpdateDiscountCommandRule : BussinessRule<UpdateDiscountCommand>
    {
        #region constructor

        private readonly IDiscountRepository _DiscountRepository;
        public UpdateDiscountCommandRule(IDiscountRepository DiscountRepository)
        {
            _DiscountRepository = DiscountRepository;
        }

        #endregion


        [RuleItem]
        public async Task Name_Must_Not_Duplicate()
        {
            if (await _DiscountRepository.IsExist(_request.Name , exceptId:_request.Id))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Discount with name '{_request.Name}' already exist");
            }
        }



    }




}

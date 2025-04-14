using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Discount.Commands.Create;
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
        public async Task Check_Name_IsNotDuplicate()
        {
            if (await _DiscountRepository.IsExist(_request.Name , exceptId:_request.Id))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Name), $"Discount with name '{_request.Name} already exist'"));
            }
        }



    }




}

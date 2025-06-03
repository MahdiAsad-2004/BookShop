using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Discount.Commands.Create
{
    public class CreateDiscountCommandRule : BussinessRule<CreateDiscountCommand>
    {
        #region constructor

        private readonly IDiscountRepository _DiscountRepository;
        public CreateDiscountCommandRule(IDiscountRepository DiscountRepository)
        {
            _DiscountRepository = DiscountRepository;
        }

        #endregion


        [RuleItem]
        public async Task Name_Must_Not_Duplicate()
        {
            if (await _DiscountRepository.IsExist(_request.Name))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Discount with name '{_request.Name}' already exist");
            }
        }

      


    }
}

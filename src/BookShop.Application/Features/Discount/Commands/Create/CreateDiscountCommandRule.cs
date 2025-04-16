using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
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
        public async Task Check_Name_IsNotDuplicate()
        {
            if (await _DiscountRepository.IsExist(_request.Name))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Name), $"Discount with name '{_request.Name}' already exist"));
            }
        }

      


    }
}

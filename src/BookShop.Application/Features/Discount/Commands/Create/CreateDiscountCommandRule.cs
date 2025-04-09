using BookShop.Application.Common.Rule;
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


        public override async Task CheckRules(CreateDiscountCommand request, bool stopOnError)
        {
            await CheckNameIsDuplicate(request);

            if (MustStop(stopOnError)) return;
        }




        private async Task CheckNameIsDuplicate(CreateDiscountCommand command)
        {
            if (await _DiscountRepository.IsExist(command.Name))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Name), $"Discount with name '{command.Name} already exist'"));
            }
        }

      


    }
}

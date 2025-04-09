using BookShop.Application.Common.Rule;
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


        public override async Task CheckRules(UpdateDiscountCommand request, bool stopOnError)
        {
            await CheckNameIsDuplicate(request);

            if (MustStop(stopOnError)) return;
        }




        private async Task CheckNameIsDuplicate(UpdateDiscountCommand command)
        {
            if (await _DiscountRepository.IsExist(command.Name , exceptId:command.Id))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Name), $"Discount with name '{command.Name} already exist'"));
            }
        }



    }




}

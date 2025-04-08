using BookShop.Application.Common.Rule;
using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Category.Commands.Update
{
    public class UpdateCategoryCommandRule : BussinessRule<UpdateCategoryCommand>
    {
        #region constructor

        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryCommandRule(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion


        public override async Task CheckRules(UpdateCategoryCommand request, bool stopOnError)
        {
            await CheckNameIsDuplicate(request);

            if (MustStop(stopOnError)) return;

            await CheckParentIdIsExist(request);
        }



        private async Task CheckNameIsDuplicate(UpdateCategoryCommand command)
        {
            if (await _categoryRepository.IsExist(command.Title,exceptId: command.Id))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Title), $"Category with title '{command.Title} already exist'"));
            }
        }
        private async Task CheckParentIdIsExist(UpdateCategoryCommand command)
        {
            if (command.ParentId != null && command.ParentId != Guid.Empty)
            {
                if (await _categoryRepository.IsExist(command.ParentId.Value) == false)
                {
                    ErrorOccured();
                    ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.ParentId), $"Parent Id with id '{command.ParentId} does not exist'"));
                }
            }
        }


    }




}

using BookShop.Application.Common.Rule;
using BookShop.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Application.Features.Category.Commands.Create
{
    public class CreateCategoryCommandRule : BussinessRule<CreateCategoryCommand>
    {
        #region constructor

        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryCommandRule(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion


        public override async Task CheckRules(CreateCategoryCommand request, bool stopOnError)
        {
            await CheckTitleIsDuplicate(request);

            if (MustStop(stopOnError)) return;

            await CheckParentIdIsExist(request);
        }




        private async Task CheckTitleIsDuplicate(CreateCategoryCommand command)
        {
            if (await _categoryRepository.IsExist(command.Title))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Title), $"Category with title '{command.Title} already exist'"));
            }
        }

        private async Task CheckParentIdIsExist(CreateCategoryCommand command)
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

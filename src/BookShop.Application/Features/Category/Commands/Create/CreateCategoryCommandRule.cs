using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.IRepositories;

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


        [RuleItem]
        public async Task Check_Title_IsNotDuplicate()
        {
            if (await _categoryRepository.IsExist(_request.Title))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Title), $"Category with title '{_request.Title}' already exist"));
            }
        }

        [RuleItem]
        public async Task Check_ParentIdIs_Exist()
        {
            if (_request.ParentId != null && _request.ParentId != Guid.Empty)
            {
                if (await _categoryRepository.IsExist(_request.ParentId.Value) == false)
                {
                    errorOccured();
                    ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.ParentId), $"Parent Id with id '{_request.ParentId}' does not exist"));
                }
            }
        }




    }
}

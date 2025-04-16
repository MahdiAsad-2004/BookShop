using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
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


        [RuleItem]
        public async Task Check_Title_IsNotDuplicate()
        {
            if (await _categoryRepository.IsExist(_request.Title,exceptId: _request.Id))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Title), $"Category with title '{_request.Title}' already exist"));
            }
        }

        [RuleItem]
        public async Task Check_ParentId_Exist()
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

using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
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
        public async Task Title_Must_Not_Be_Duplicate()
        {
            if (await _categoryRepository.IsExist(_request.Title))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Title), $"Category with title '{_request.Title}' already exist");
            }
        }

        [RuleItem]
        public async Task ParentId_Must_Exist()
        {
            if (_request.ParentId != null && _request.ParentId != Guid.Empty)
            {
                if (await _categoryRepository.IsExist(_request.ParentId.Value) == false)
                {
                    errorOccured();
                    addErrorDetail(ErrorCode.Not_Found, nameof(_request.ParentId), $"Parent Id with id '{_request.ParentId}' does not exist");
                }
            }
        }




    }
}

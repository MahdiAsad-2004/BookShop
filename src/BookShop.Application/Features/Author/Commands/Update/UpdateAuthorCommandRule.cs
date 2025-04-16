using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Author.Commands.Update
{
    public class UpdateAuthorCommandRule : BussinessRule<UpdateAuthorCommand>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        public UpdateAuthorCommandRule(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        [RuleItem]
        public async Task Check_Name_IsNotDuplicate()
        {
            if (await _authorRepository.IsExist(_request.Name,exceptId: _request.Id))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Name), $"Author with name '{_request.Name}' already exist"));
            }
        }



    }




}

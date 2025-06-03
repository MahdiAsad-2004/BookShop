using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Domain.Enums;
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
        public async Task Name_Must_Not_Duplicate()
        {
            if (await _authorRepository.IsExist(_request.Name,exceptId: _request.Id))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Author with name '{_request.Name}' already exist");
            }
        }



    }




}

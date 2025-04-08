using BookShop.Application.Common.Rule;
using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Author.Commands.Update
{
    public class UpdateCategoryCommandRule : BussinessRule<UpdateAuthorCommand>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        public UpdateCategoryCommandRule(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        public override async Task CheckRules(UpdateAuthorCommand request, bool stopOnError)
        {
            await CheckNameIsDuplicate(request);
        }



        private async Task CheckNameIsDuplicate(UpdateAuthorCommand command)
        {
            if (await _authorRepository.IsExist(command.Name,exceptId: command.Id))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Name), $"Author with name '{command.Name} already exist'"));
            }
        }



    }




}

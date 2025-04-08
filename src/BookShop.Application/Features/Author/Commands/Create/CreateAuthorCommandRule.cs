using BookShop.Application.Common.Rule;
using BookShop.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Application.Features.Author.Commands.Create
{
    public class CreateAuthorCommandRule : BussinessRule<CreateAuthorCommand>
    {

        #region constructor

        private readonly IAuthorRepository _authorRepository;
        public CreateAuthorCommandRule(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        public override async Task CheckRules(CreateAuthorCommand request, bool stopOnError)
        {
            await CheckNameIsDuplicate(request);
        }




        private async Task CheckNameIsDuplicate(CreateAuthorCommand command)
        {
            if(await _authorRepository.IsExist(command.Name))
            {
                ErrorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(command.Name), $"Author with name '{command.Name} already exist'"));
            }
        }




    }
}

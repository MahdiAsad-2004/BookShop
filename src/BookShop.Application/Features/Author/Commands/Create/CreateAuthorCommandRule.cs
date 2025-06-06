﻿using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Common;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

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



        [RuleItem]
        public async Task Check_Name_IsNotDuplicate()
        {
            if (await _authorRepository.IsExist(_request.Name))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Author with name '{_request.Name}' already exist");
            }
        }




    }
}

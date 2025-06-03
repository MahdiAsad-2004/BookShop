using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Translator.Commands.Update
{
    public class UpdateTranslatorCommandRule : BussinessRule<UpdateTranslatorCommand>
    {
        #region constructor

        private readonly ITranslatorRepository _authorRepository;
        public UpdateTranslatorCommandRule(ITranslatorRepository authorRepository)
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
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Translator with name '{_request.Name}' already exist");
            }
        }



    }




}

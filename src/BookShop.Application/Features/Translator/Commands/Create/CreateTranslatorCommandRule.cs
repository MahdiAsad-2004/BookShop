using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Translator.Commands.Create
{
    public class CreateTranslatorCommandRule : BussinessRule<CreateTranslatorCommand>
    {

        #region constructor

        private readonly ITranslatorRepository _translatorRepository;
        public CreateTranslatorCommandRule(ITranslatorRepository translatorRepository)
        {
            _translatorRepository = translatorRepository;
        }

        #endregion



        [RuleItem]
        public async Task ImageFile_Must_Not_Null()
        {
            if(_request.ImageFile == null)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Required_Field, nameof(_request.ImageFile), "{PropertyName} can not be null");
            }
        }

        [RuleItem]
        public async Task Name_Must_Not_Duplicate()
        {
            if(await _translatorRepository.IsExist(_request.Name))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Name), $"Translator with name '{_request.Name}' already exist");
            }
        }




    }
}

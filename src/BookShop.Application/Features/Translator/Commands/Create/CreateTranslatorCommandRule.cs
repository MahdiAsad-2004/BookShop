using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
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
        public async Task Check_ImageFile_IsNotNull()
        {
            if(_request.ImageFile == null)
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.ImageFile), "{PropertyName} can not be null"));
            }
        }

        [RuleItem]
        public async Task Check_Name_IsNotDuplicate()
        {
            if(await _translatorRepository.IsExist(_request.Name))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Name), $"Translator with name '{_request.Name}' already exist"));
            }
        }




    }
}

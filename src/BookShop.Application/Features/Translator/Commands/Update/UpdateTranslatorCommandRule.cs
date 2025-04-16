using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Translator.Commands.Create;
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
        public async Task Check_Name_IsNotDuplicate()
        {
            if (await _authorRepository.IsExist(_request.Name,exceptId: _request.Id))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Name), $"Translator with name '{_request.Name}' already exist"));
            }
        }



    }




}

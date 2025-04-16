using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Publisher.Commands.Create
{
    public class CreatePublisherCommandRule : BussinessRule<CreatePublisherCommand>
    {
        #region constructor

        private readonly IPublisherRepository _publisherRepository;
        public CreatePublisherCommandRule(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        #endregion



        [RuleItem]
        public async Task Check_ImageFile_IsNotNull()
        {
            if (_request.ImageFile == null)
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.ImageFile), "{PropertyName} can not be null"));
            }
        }

        [RuleItem]
        public async Task Check_Title_IsNotDuplicate()
        {
            if (await _publisherRepository.IsExist(_request.Title))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Title), $"Publisher with title '{_request.Title}' already exist"));
            }
        }

        




    }
}

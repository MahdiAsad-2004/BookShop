using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
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
        public async Task ImageFile_Must_NotNull()
        {
            if (_request.ImageFile == null)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Required_Field, nameof(_request.ImageFile), "{PropertyName} can not be null");
            }
        }

        [RuleItem]
        public async Task Title_Must_Not_Be_Duplicate()
        {
            if (await _publisherRepository.IsExist(_request.Title))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Title), $"Publisher with title '{_request.Title}' already exist");
            }
        }

        




    }
}

using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Publisher.Commands.Update
{
    public class UpdatePublisherCommandRule : BussinessRule<UpdatePublisherCommand>
    {
        #region constructor

        private readonly IPublisherRepository _publisherRepository;
        public UpdatePublisherCommandRule(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        #endregion



        [RuleItem]
        public async Task Title_Must_Not_Duplicate()
        {
            if (await _publisherRepository.IsExist(_request.Title,exceptId: _request.Id))
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry, nameof(_request.Title), $"Publisher with title '{_request.Title}' already exist");
            }
        }

    }




}

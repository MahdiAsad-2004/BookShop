using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.Publisher.Commands.Create;
using BookShop.Application.Features.Publisher.Commands.Update;
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
        public async Task Check_Title_IsNotDuplicate()
        {
            if (await _publisherRepository.IsExist(_request.Title,exceptId: _request.Id))
            {
                errorOccured();
                ValidationErrors.Add(new Domain.Exceptions.ValidationError(nameof(_request.Title), $"Publisher with title '{_request.Title}' already exist"));
            }
        }

    }




}

using BookShop.Application.Common.Rule;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Book.Commands.Update
{
    public class UpdateBookCommandRule : BussinessRule<UpdateBookCommand>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITranslatorRepository _translatorRepository;
        public UpdateBookCommandRule(ICategoryRepository categoryRepository, IPublisherRepository publisherRepository,
            IProductRepository productRepository, IAuthorRepository authorRepository, ITranslatorRepository translatorRepository)
        {
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
            _productRepository = productRepository;
            _authorRepository = authorRepository;
            _translatorRepository = translatorRepository;
        }

        #endregion



        public async override Task CheckRules(UpdateBookCommand updateBookCommand, bool stopOnError)
        {
            await CheckCategoryIdExist(updateBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckPublisherIdExist(updateBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckProductTitleIsDuplicate(updateBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckAuthorIdsExist(updateBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckTrnaslatorIdExist(updateBookCommand);
        }





        private async Task CheckProductTitleIsDuplicate(UpdateBookCommand command)
        {
            if (await _productRepository.IsExist(command.Product_Title, exceptId: command.Id) == true)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.Product_Title), $"Product with title '{command.Product_Title}' already exist"));
            }
        }
        private async Task CheckCategoryIdExist(UpdateBookCommand command)
        {
            if (command.Product_CategoryId != null)
            {
                if (await _categoryRepository.IsExist(command.Product_CategoryId.Value) == false)
                {
                    ErrorOccured();
                    ValidationErrors.Add(new ValidationError(nameof(command.Product_CategoryId),
                        $"Category with id '{command.Product_CategoryId}' does not exist"));
                }
            }

        }
        private async Task CheckPublisherIdExist(UpdateBookCommand command)
        {
            if (await _publisherRepository.IsExist(command.PublisherId) == false)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.PublisherId), $"Publisher with id '{command.PublisherId}' does not exist"));
            }
        }
        private async Task CheckAuthorIdsExist(UpdateBookCommand command)
        {
            if (await _authorRepository.AreExist(command.AuthorIds) == false)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.AuthorIds), $"Some Authors does not exist"));
            }
        }
        private async Task CheckTrnaslatorIdExist(UpdateBookCommand command)
        {
            if (command.TranslatorId != null)
            {
                if (await _translatorRepository.IsExist(command.TranslatorId.Value) == false)
                {
                    ErrorOccured();
                    ValidationErrors.Add(new ValidationError(nameof(command.TranslatorId), $"Translator with id '{command.TranslatorId}' does not exist"));
                }
            }
        }







    }

}

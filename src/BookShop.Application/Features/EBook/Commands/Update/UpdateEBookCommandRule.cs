using BookShop.Application.Common.Rules;
using BookShop.Application.Common.Ruless;
using BookShop.Application.Features.EBook.Commands.Update;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Book.Commands.Update
{
    public class UpdateEBookCommandRule : BussinessRule<UpdateEBookCommand>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITranslatorRepository _translatorRepository;
        private readonly IEBookRepository _ebookRepository;
        public UpdateEBookCommandRule(ICategoryRepository categoryRepository, IPublisherRepository publisherRepository,IProductRepository productRepository, 
            IAuthorRepository authorRepository, ITranslatorRepository translatorRepository, IEBookRepository ebookRepository)
        {
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
            _productRepository = productRepository;
            _authorRepository = authorRepository;
            _translatorRepository = translatorRepository;
            _ebookRepository = ebookRepository;
        }

        #endregion




        [RuleItem]
        public async Task Product_Title_Must_Not_Duplicate()
        {
            Guid productId = (await _ebookRepository.Get(_request.Id)).ProductId;
            if (await _productRepository.IsExist(_request.Product_Title , exceptId:productId) == true)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Duplicate_Entry , nameof(_request.Product_Title), $"Product with title '{_request.Product_Title}' already exist");
            }
        }

        [RuleItem]
        public async Task Product_CategoryId_Must_Exist()
        {
            if (_request.Product_CategoryId != null)
            {
                if (await _categoryRepository.IsExist(_request.Product_CategoryId.Value) == false)
                {
                    errorOccured();
                    addErrorDetail(ErrorCode.Not_Found, nameof(_request.Product_CategoryId), $"Category with id '{_request.Product_CategoryId}' not found");
                }
            }

        }

        [RuleItem]
        public async Task PublisherId_Must_Exist()
        {
            if (await _publisherRepository.IsExist(_request.PublisherId) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found, nameof(_request.PublisherId), $"Publisher with id '{_request.PublisherId}' not found");
            }
        }

        [RuleItem]
        public async Task AuthorIds_Must_Exist()
        {
            if (await _authorRepository.AreExist(_request.AuthorIds) == false)
            {
                errorOccured();
                addErrorDetail(ErrorCode.Not_Found, nameof(_request.AuthorIds), $"Some Authors not found");
            }
        }

        [RuleItem]
        public async Task TrnaslatorId_Must_Exist()
        {
            if (_request.TranslatorId != null)
            {
                if (await _translatorRepository.IsExist(_request.TranslatorId.Value) == false)
                {
                    errorOccured();
                    addErrorDetail(ErrorCode.Not_Found, nameof(_request.TranslatorId), $"Translator with id '{_request.TranslatorId}' not found");
                }
            }
        }





    }

}

﻿using BookShop.Application.Common.Rule;
using BookShop.Application.Features.Book.Commands.Update;
using BookShop.Domain.Common;
using BookShop.Domain.Exceptions;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Book.Commands.Create
{
    public class CreateBookCommandRule : BussinessRule<CreateBookCommand>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITranslatorRepository _translatorRepository;
        public CreateBookCommandRule(ICategoryRepository categoryRepository, IPublisherRepository publisherRepository,
            IProductRepository productRepository, IAuthorRepository authorRepository, ITranslatorRepository translatorRepository)
        {
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
            _productRepository = productRepository;
            _authorRepository = authorRepository;
            _translatorRepository = translatorRepository;
        }

        #endregion



        public async override Task CheckRules(CreateBookCommand createBookCommand, bool stopOnError)
        {
            await CheckProductImageFileIsNotNull(createBookCommand);

            if (MustStop(stopOnError)) return;
            
            await CheckCategoryIdExist(createBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckPublisherIdExist(createBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckProductTitleIsDuplicate(createBookCommand);

            if (MustStop(stopOnError)) return;

            await CheckAuthorIdsExist(createBookCommand);
        
            if (MustStop(stopOnError)) return;

            await CheckTrnaslatorIdExist(createBookCommand);
        }






        
        private async Task CheckProductImageFileIsNotNull(CreateBookCommand command)
        {
            if(command.Product_ImageFile == null)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.Product_ImageFile),
                    $"Image File can not be null"));
            }
        }
        private async Task CheckCategoryIdExist(CreateBookCommand command)
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
        private async Task CheckPublisherIdExist(CreateBookCommand command)
        {
            if (await _publisherRepository.IsExist(command.PublisherId) == false)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.PublisherId), $"Publisher with id '{command.PublisherId}' does not exist"));
            }
        }
        private async Task CheckProductTitleIsDuplicate(CreateBookCommand command)
        {
            if (await _productRepository.IsExist(command.Product_Title) == true)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.Product_Title), $"Product with title '{command.Product_Title}' already exist"));
            }
        }
        private async Task CheckAuthorIdsExist(CreateBookCommand command)
        {
            if (await _authorRepository.AreExist(command.AuthorIds) == false)
            {
                ErrorOccured();
                ValidationErrors.Add(new ValidationError(nameof(command.AuthorIds), $"Some Authors does not exist"));
            }
        }
        private async Task CheckTrnaslatorIdExist(CreateBookCommand command)
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

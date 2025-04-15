using BookShop.Application.Authorization;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Book.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Book.Commands.Update
{
    [RequiredPermission(PermissionConstants.UpdateBook)]
    public class UpdateBookCommand : IRequest<Result<Empty>>, IRequest
    {
        public Guid Id { get; set; }
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public int Product_NumberOfInventory { get; set; }
        public string Product_DescriptionHtml { get; set; }
        public Guid? Product_CategoryId { get; set; }
        public int NumberOfPages { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Language Language { get; set; }
        public DateTime PublishYear { get; set; }
        public float? WeightInGram { get; set; }
        public int? Edition { get; set; }
        public IFormFile? Product_ImageFile { get; set; }
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }
        public Guid[] AuthorIds { get; set; }
    }



    public class UpdateBookCommandHanler : IRequestHandler<UpdateBookCommand, Result<Empty>>
    {
        #region constructor

        private readonly IBookRepository _bookRepository;
        private readonly IAuthor_BookRepository _author_BookRepository;
        public UpdateBookCommandHanler(IBookRepository bookRepository, IAuthor_BookRepository author_BookRepository)
        {
            _bookRepository = bookRepository;
            _author_BookRepository = author_BookRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            //FetchEntity
            E.Book book = await _bookRepository.Get(request.Id, new Domain.QueryOptions.BookQueryOption
            {
                IncludeProduct = true
            });

            //Mapping
            book = BookMapper.ToBookAndProduct(request, book);

            //SaveFile
            string? imageName = book.Product.ImageName;
            bool fileSaved = false;
            if (request.Product_ImageFile != null)
            {
                imageName = $"book-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.Product_ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Product_Images, request.Product_ImageFile.OpenReadStream());
            }
            if (fileSaved && book.Product.ImageName != null)
            {
                await FileExtensions.DeleteFileIfExist(book.Product.ImageName, PathExtensions.Product_Images);
            }
            book.Product.ImageName = fileSaved ? imageName : null;

            //Save
            await _bookRepository.Update(book);
            await _author_BookRepository.InsertNewOnesAndDeleteAnothers(request.Id, request.AuthorIds);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

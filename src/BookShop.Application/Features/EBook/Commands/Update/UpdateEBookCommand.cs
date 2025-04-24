using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Book.Mapping;
using BookShop.Application.Features.EBook.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.EBook.Commands.Update
{
    [RequiredPermission(PermissionConstants.EBook.Update)]
    public class UpdateEBookCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid Id { get; set; }
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public int Product_NumberOfInventory { get; set; }
        public string Product_DescriptionHtml { get; set; }
        public Guid? Product_CategoryId { get; set; }
        public int NumberOfPages { get; set; }
        public Language Language { get; set; }
        public DateTime PublishYear { get; set; }
        public int? Edition { get; set; }
        public IFormFile? Product_ImageFile { get; set; }
        public IFormFile? EBookFile { get; set; }
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }
        public Guid[] AuthorIds { get; set; }

    }



    public class UpdateEBookCommandHanler : IRequestHandler<UpdateEBookCommand, Result<Empty>>
    {
        #region constructor

        private readonly IEBookRepository _ebookRepository;
        private readonly IAuthor_EBookRepository _author_EBookRepository;
        public UpdateEBookCommandHanler(IEBookRepository ebookRepository, IAuthor_EBookRepository author_EBookRepository)
        {
            _ebookRepository = ebookRepository;
            _author_EBookRepository = author_EBookRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateEBookCommand request, CancellationToken cancellationToken)
        {
            //FetchEntity
            E.EBook ebook = await _ebookRepository.Get(request.Id, new EBookQueryOption
            {
                IncludeProduct = true
            });

            //Mapping
            ebook = EBookMapper.ToEBookAndProduct(request, ebook);

            //Store File
            string fileName = ebook.FileName;
            bool fileSaved = false;
            if (request.EBookFile != null)
            {
                fileName = $"ebook-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.EBookFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(fileName, PathExtensions.EBook_Files, request.EBookFile.OpenReadStream());
            }
            if (fileSaved)
            {
                await FileExtensions.DeleteFileIfExist(ebook.FileName, PathExtensions.EBook_Files);
                ebook.FileName = fileName;
            }

            //Store Image
            string? imageName = ebook.Product.ImageName;
            bool imageSaved = false;
            if (request.Product_ImageFile != null)
            {
                imageName = $"ebook-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.Product_ImageFile.FileName)}";
                imageSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Product_Images, request.Product_ImageFile.OpenReadStream());
            }
            if (imageSaved && ebook.Product.ImageName != null)
            {
                await FileExtensions.DeleteFileIfExist(ebook.Product.ImageName, PathExtensions.Product_Images);
            }
            ebook.Product.ImageName = imageSaved ? imageName : null;

            //Update Entity
            await _ebookRepository.Update(ebook);
            await _author_EBookRepository.InsertNewOnesAndDeleteAnothers(request.Id, request.AuthorIds);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

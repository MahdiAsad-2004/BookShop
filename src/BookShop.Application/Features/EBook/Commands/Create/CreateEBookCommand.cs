using BookShop.Application.Authorization;
using BookShop.Application.Extensions;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using EBookShop.Application.Features.EBook.Mapping;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.EBook.Commands.Create
{
    [RequiredPermission(PermissionConstants.AddEBook)]
    public class CreateEBookCommand : IRequest<Result<Empty>>, IRequest
    {
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public int Product_NumberOfInventory { get; set; }
        public string Product_DescriptionHtml { get; set; }
        public Guid? Product_CategoryId { get; set; }
        public int NumberOfPages { get; set; }
        public Language Language { get; set; }
        public DateTime PublishYear { get; set; }
        public int? Edition { get; set; }
        public IFormFile Product_ImageFile { get; set; }
        public IFormFile EBookFile { get; set; }
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }
        public Guid[] AuthorIds { get; set; }    

    }


    public class CreateEBookCommandHandler : IRequestHandler<CreateEBookCommand, Result<Empty>>
    {
        #region constuctor  

        private readonly IEBookRepository _ebookRepository;
        public CreateEBookCommandHandler(IEBookRepository ebookRepository)
        {
            _ebookRepository = ebookRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateEBookCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.EBook ebook;
            E.Product product;
            EBookMapper.ToEBookAndProduct(request, out ebook, out product);

            //StoreFile
            string fileName = $"ebook-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.EBookFile.FileName)}";
            bool fileSaved = await FileExtensions.SaveFile(fileName, PathExtensions.EBook_Files, request.Product_ImageFile.OpenReadStream());
            if (fileSaved == false)
                throw new ApplicationException("EBook file does not save");
            EBookFileFormat eBookFileFormat;
            Enum.TryParse(Path.GetExtension(request.EBookFile.FileName).Remove(0, 1), out eBookFileFormat);
            ebook.FileName = fileName;
            ebook.FileSize_KB = request.EBookFile.Length / 1024;
            ebook.FileFormat = eBookFileFormat;

            //Store image
            string imageName = $"ebook-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.Product_ImageFile.FileName)}";
            bool imageSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Product_Images, request.Product_ImageFile.OpenReadStream());
            product.ImageName = imageSaved ? imageName : null;
            
            //Add Entity
            await _ebookRepository.Add(ebook, product, request.AuthorIds);

            return new Result<Empty>(Empty.New, true);
        }
    }

}

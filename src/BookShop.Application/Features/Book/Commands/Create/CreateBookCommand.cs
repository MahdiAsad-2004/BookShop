using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Common.Response;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Book.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Book.Commands.Create
{
    [RequiredPermission(PermissionConstants.AddUser)]
    public class CreateBookCommand : IRequest<Result<Empty>>, IRequest
    {
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
        public Guid PublisherId { get; set; }
        public IFormFile Product_ImageFile { get; set; }
        public Guid[] AuthorIds { get; set; }
    }


    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<Empty>>
    {
        #region constuctor  

        private readonly IBookRepository _bookRepository;
        public CreateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Book book;
            Domain.Entities.Product product;
            BookMapper.ToBookAndProduct(request, out book, out product);

            string imageName = $"book-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.Product_ImageFile.FileName)}";
            bool fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Product_Images, request.Product_ImageFile.OpenReadStream());
            product.ImageName = fileSaved ? imageName : null;

            await _bookRepository.Add(book, product, request.AuthorIds);

            return new Result<Empty>(Empty.New, true);
        }
    }

}

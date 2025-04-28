using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Author.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Author.Commands.Create
{
    [RequiredPermission(PermissionConstants.Author.Add)]
    public class CreateAuthorCommand : IRequest<Result<Empty>> , IValidatableRquest
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



    public class CreateAuthorCommandHanler : IRequestHandler<CreateAuthorCommand, Result<Empty>>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        public CreateAuthorCommandHanler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.Author author = AuthorMapper.ToAuthor(request);

            //StoreFile
            string? imageName = null;
            bool fileSaved = false; 
            if (request.ImageFile != null) 
            {
                imageName = $"author-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Author.Images, request.ImageFile.OpenReadStream());
            }
            author.ImageName = fileSaved ? imageName : null;

            //Save
            await _authorRepository.Add(author);


            return new Result<Empty>(Empty.New, true);
        }
    }

}

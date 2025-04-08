using BookShop.Application.Authorization;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Author.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Author.Commands.Update
{
    [RequiredPermission(PermissionConstants.UpdateAuthor)]
    public class UpdateAuthorCommand : IRequest<Result<Empty>>, IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



    public class UpdateAuthorCommandHanler : IRequestHandler<UpdateAuthorCommand, Result<Empty>>
    {
        #region constructor

        private readonly IAuthorRepository _authorRepository;
        public UpdateAuthorCommandHanler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            //FetchEntity
            E.Author author = await _authorRepository.Get(request.Id);

            //Mapping
            author = AuthorMapper.ToAuthor(author, request);

            //SaveFile
            string? imageName = author.ImageName;
            bool fileSaved = false;
            if (request.ImageFile != null)
            {
                imageName = $"author-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Author_Images, request.ImageFile.OpenReadStream());
            }
            if (fileSaved && author.ImageName != null)
            {
                await FileExtensions.DeleteFile(author.ImageName, PathExtensions.Author_Images);
            }
            author.ImageName = fileSaved ? imageName : null;

            //Save
            await _authorRepository.Update(author);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

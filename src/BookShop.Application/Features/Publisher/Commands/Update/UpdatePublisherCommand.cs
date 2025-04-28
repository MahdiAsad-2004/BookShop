using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Publisher.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Publisher.Commands.Update
{
    [RequiredPermission(PermissionConstants.Publisher.Update)]
    public class UpdatePublisherCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IFormFile? ImageFile { get; set; }

    }



    public class UpdatePublisherCommandHanler : IRequestHandler<UpdatePublisherCommand, Result<Empty>>
    {
        #region constructor

        private readonly IPublisherRepository _publisherRepository;
        public UpdatePublisherCommandHanler(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
        {
            //Fetch Entity
            E.Publisher publisher = await _publisherRepository.Get(request.Id);

            //Mapping
            publisher = PublisherMapper.ToPublisher(publisher, request);

            //Store Image
            string? imageName = publisher.ImageName;
            bool imageSaved = false;
            if (request.ImageFile != null)
            {
                imageName = $"publisher-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                imageSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Publisher.Images, request.ImageFile.OpenReadStream());
            }
            if (imageSaved && publisher.ImageName != null)
            {
                await FileExtensions.DeleteFileIfExist(publisher.ImageName, PathExtensions.Publisher.Images);
            }
            publisher.ImageName = imageSaved ? imageName : null;

            //Update Entity
            await _publisherRepository.Update(publisher);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Publisher.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Publisher.Commands.Create
{
    [RequiredPermission(PermissionConstants.Publisher.Add)]
    public class CreatePublisherCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }

    }

    public class CreatePublisherCommandHanler : IRequestHandler<CreatePublisherCommand,Result<Empty>>
    {
        #region constructor

        private readonly IPublisherRepository _publisherRepository;
        public CreatePublisherCommandHanler(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.Publisher publisher = PublisherMapper.ToPublisher(request);

            //Store File
            string imageName = $"publisher-{Guid.NewGuid().ToString().Substring(0,8)}{Path.GetExtension(request.ImageFile.FileName)}";
            bool fileSvaed = await FileExtensions.SaveFile(imageName, PathExtensions.Publisher.Images, request.ImageFile.OpenReadStream());
            publisher.ImageName = fileSvaed ? imageName : null;

            //Add Entity
            await _publisherRepository.Add(publisher);

            return new Result<Empty>(Empty.New, true);
        }


    }






}

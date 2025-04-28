using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Translator.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Translator.Commands.Create
{
    [RequiredPermission(PermissionConstants.Translator.Add)]
    public class CreateTranslatorCommand : IRequest<Result<Empty>> , IValidatableRquest
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public IFormFile ImageFile { get; set; }
    }



    public class CreateTranslatorCommandHanler : IRequestHandler<CreateTranslatorCommand, Result<Empty>>
    {
        #region constructor

        private readonly ITranslatorRepository _authorRepository;
        public CreateTranslatorCommandHanler(ITranslatorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateTranslatorCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.Translator translator = TranslatorMapper.ToTranslator(request);

            //Store File
            string? imageName = null;
            bool fileSaved = false; 
            if (request.ImageFile != null) 
            {
                imageName = $"translator-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Translator.Images, request.ImageFile.OpenReadStream());
            }
            translator.ImageName = fileSaved ? imageName : null;

            //Add Entity
            await _authorRepository.Add(translator);


            return new Result<Empty>(Empty.New, true);
        }
    }

}

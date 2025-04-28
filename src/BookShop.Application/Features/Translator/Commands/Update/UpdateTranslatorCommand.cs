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

namespace BookShop.Application.Features.Translator.Commands.Update
{
    [RequiredPermission(PermissionConstants.Translator.Update)]
    public class UpdateTranslatorCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



    public class UpdateTranslatorCommandHanler : IRequestHandler<UpdateTranslatorCommand, Result<Empty>>
    {
        #region constructor

        private readonly ITranslatorRepository _translatorRepository;
        public UpdateTranslatorCommandHanler(ITranslatorRepository translatorRepository)
        {
            _translatorRepository = translatorRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateTranslatorCommand request, CancellationToken cancellationToken)
        {
            //Fetch Entity
            E.Translator translator = await _translatorRepository.Get(request.Id);

            //Mapping
            translator = TranslatorMapper.ToTranslator(translator, request);

            //Store Image
            string? imageName = translator.ImageName;
            bool fileSaved = false;
            if (request.ImageFile != null)
            {
                imageName = $"translator-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Translator.Images, request.ImageFile.OpenReadStream());
            }
            if (fileSaved && translator.ImageName != null)
            {
                await FileExtensions.DeleteFileIfExist(translator.ImageName, PathExtensions.Translator.Images);
            }
            translator.ImageName = fileSaved ? imageName : null;

            //Update Entity
            await _translatorRepository.Update(translator);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

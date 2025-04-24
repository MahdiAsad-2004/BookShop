using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Category.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Category.Commands.Update
{
    [RequiredPermission(PermissionConstants.Categoory.Update)]
    public class UpdateCategoryCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



    public class UpdateCategoryCommandHanler : IRequestHandler<UpdateCategoryCommand, Result<Empty>>
    {
        #region constructor

        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryCommandHanler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            //FetchEntity
            E.Category category = await _categoryRepository.Get(request.Id);

            //Mapping
            category = CategoryMapper.ToCategory(category, request);

            //SaveFile
            string? imageName = category.ImageName;
            bool fileSaved = false;
            if (request.ImageFile != null)
            {
                imageName = $"category-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(request.ImageFile.FileName)}";
                fileSaved = await FileExtensions.SaveFile(imageName, PathExtensions.Category_Images, request.ImageFile.OpenReadStream());
            }
            if (fileSaved && category.ImageName != null)
            {
                await FileExtensions.DeleteFileIfExist(category.ImageName, PathExtensions.Category_Images);
            }
            category.ImageName = fileSaved ? imageName : null;

            //Save
            await _categoryRepository.Update(category);

            return new Result<Empty>(Empty.New, true);
        }



    }



}

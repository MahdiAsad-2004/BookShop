using BookShop.Application.Authorization;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Category.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Category.Commands.Create
{
    [RequiredPermission(PermissionConstants.AddCategory)]
    public class CreateCategoryCommand : IRequest<Result<Empty>>, IRequest
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public IFormFile ImageFile { get; set; }

    }



    public class CreateCategoryCommandHanler : IRequestHandler<CreateCategoryCommand,Result<Empty>>
    {
        #region constructor

        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryCommandHanler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.Category category = CategoryMapper.ToCategory(request);

            //StoreFile
            string imageName = $"category-{Guid.NewGuid().ToString().Substring(0,8)}{Path.GetExtension(request.ImageFile.FileName)}";
            bool fileSvaed = await FileExtensions.SaveFile(imageName, PathExtensions.Category_Images, request.ImageFile.OpenReadStream());
            category.ImageName = fileSvaed ? imageName : null;

            //SaveEntity
            await _categoryRepository.Add(category);

            return new Result<Empty>(Empty.New, true);
        }


    }






}

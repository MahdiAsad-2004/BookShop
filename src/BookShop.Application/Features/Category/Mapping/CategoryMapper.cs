using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Application.Features.Category.Commands.Update;

namespace BookShop.Application.Features.Category.Mapping
{
    public static class CategoryMapper
    {
        public static E.Category ToCategory(CreateCategoryCommand command)
        {
            return new E.Category
            {
                Title = command.Title,
                ParentId = command.ParentId,
            };
        }


        
        public static E.Category ToCategory(E.Category category,UpdateCategoryCommand command)
        {
            category.Title = command.Title;
            category.ParentId = command.ParentId;
            return category;
        }



    }
}

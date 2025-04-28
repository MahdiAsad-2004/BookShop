using BookShop.Application.Extensions;
using BookShop.WebApi.Models.Author;

namespace BookShop.WebApi.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorModel ToModel(E.Author author)
        {
            string imageName = string.Empty;
            if (author.ImageName != null)
            {
                imageName = author.ImageName;
            }
            else
            {
                imageName += author.Gender == Gender.Man ? PathExtensions.Author.Man_DefaultImageName : PathExtensions.Author.Woman_DefaultImageName;
            }
            return new AuthorModel(author.Id, author.Name, Path.Combine(PathExtensions.Author.Images , imageName));
        }


        public static List<AuthorModel> ToModel(List<E.Author> authors)
        {
            return authors.Select(a => ToModel(a)).ToList();
        }


    }
}

using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Author.Commands.Update;

namespace BookShop.Application.Features.Author.Mapping
{
    public static class AuthorMapper
    {

        public static E.Author ToAuthor(CreateAuthorCommand command)
        {
            return new E.Author
            {
                Name = command.Name,
                Gender = command.Gender,
            };
        }


        public static E.Author ToAuthor(E.Author author,UpdateAuthorCommand command)
        {
            author.Name = command.Name; 
            author.Gender = command.Gender;
            return author;
        }
        


    }
}

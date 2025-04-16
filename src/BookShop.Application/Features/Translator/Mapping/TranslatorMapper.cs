using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Update;

namespace BookShop.Application.Features.Translator.Mapping
{
    public static class TranslatorMapper
    {

        public static E.Translator ToTranslator(CreateTranslatorCommand command)
        {
            return new E.Translator
            {
                Name = command.Name,
                Gender = command.Gender,
            };
        }


        public static E.Translator ToTranslator(E.Translator author,UpdateTranslatorCommand command)
        {
            author.Name = command.Name; 
            author.Gender = command.Gender;
            return author;
        }
        


    }
}

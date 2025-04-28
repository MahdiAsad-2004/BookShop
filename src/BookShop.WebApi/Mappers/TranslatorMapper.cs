using BookShop.Application.Extensions;
using BookShop.WebApi.Models.Translator;

namespace BookShop.WebApi.Mappers
{
    public static class TranslatorMapper
    {
        public static TranslatorModel ToModel(E.Translator translator)
        {
            string imageName = translator.ImageName;
            if (imageName == null) 
            {
                imageName = translator.Gender == Gender.Man ? PathExtensions.Translator.Man_DefaultImageName : PathExtensions.Translator.Woman_DefaultImageName;
            }
            return new TranslatorModel(translator.Id, translator.Name, Path.Combine(PathExtensions.Translator.Images , imageName));
        }


        public static List<TranslatorModel> ToModel(List<E.Translator> authors)
        {
            return authors.Select(a => ToModel(a)).ToList();
        }

    }
}

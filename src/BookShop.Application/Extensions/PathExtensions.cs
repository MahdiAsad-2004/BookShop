
namespace BookShop.Application.Extensions
{
    public class PathExtensions
    {
        private static string Directory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string Product_Images = Path.Combine(Directory, "media", "product", "images");
        public static readonly string Product_DefaultImageName = "product.png";

        
        public static readonly string Author_Images = Path.Combine(Directory, "media", "author", "images");
        public static readonly string Author_Man_DefaultImageName = "author-man.png";
        public static readonly string Author_Woman_DefaultImageName = "author-woman.png";

        
        public static readonly string Category_Images = Path.Combine(Directory, "media", "category", "images");
        public static readonly string Category_DefaultImageName = "category.png";

        
        public static readonly string EBook_Files = Path.Combine(Directory, "media", "ebook", "files");

        
        public static readonly string Publisher_Images = Path.Combine(Directory, "media", "publisher", "images");
        public static readonly string Publisher_DefaultImageName = "publisher.png";


        public static readonly string Translator_Images = Path.Combine(Directory, "media", "translator", "images");
        public static readonly string Translator_Man_DefaultImageName = "translator-man.png";
        public static readonly string Translator_Woman_DefaultImageName = "translator-woman.png";


    }
}

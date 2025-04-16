
namespace BookShop.Application.Extensions
{
    public class PathExtensions
    {
        private static string Directory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string Product_Images = Path.Combine(Directory, "staticFiles", "product", "images");
        public static readonly string Product_DefaultImageName = "product.png";

        
        public static readonly string Author_Images = Path.Combine(Directory, "staticFiles", "author", "images");
        public static readonly string Author_Man_DefaultImageName = "author-man.png";
        public static readonly string Author_Woman_DefaultImageName = "author-woman.png";

        
        public static readonly string Category_Images = Path.Combine(Directory, "staticFiles", "category", "images");
        public static readonly string Category_DefaultImageName = "category.png";

        
        public static readonly string EBook_Files = Path.Combine(Directory, "staticFiles", "ebook", "files");

        
        public static readonly string Publisher_Images = Path.Combine(Directory, "staticFiles", "publisher", "images");
        public static readonly string Publisher_DefaultImageName = "publisher.png";


        public static readonly string Translator_Images = Path.Combine(Directory, "staticFiles", "translator", "images");
        public static readonly string Translator_Man_DefaultImageName = "translator-man.png";
        public static readonly string Translator_Woman_DefaultImageName = "translator-woman.png";


    }
}

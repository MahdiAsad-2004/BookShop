
namespace BookShop.Application.Extensions
{
    public class PathExtensions
    {
        private static string Directory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string Product_Images = Path.Combine(Directory, "staticFiles", "product", "images");
        public static readonly string Product_DefaultImageName = "product.png";

        public static readonly string Author_Images = Path.Combine(Directory, "staticFiles", "author", "images");
        public static readonly string Author_DefaultImageName = "author.png";

        public static readonly string Category_Images = Path.Combine(Directory, "staticFiles", "category", "images");
        public static readonly string Category_DefaultImageName = "category.png";

        public static readonly string EBook_Files = Path.Combine(Directory, "staticFiles", "ebook", "files");
        



    }
}

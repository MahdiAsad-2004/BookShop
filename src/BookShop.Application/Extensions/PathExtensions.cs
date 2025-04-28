namespace BookShop.Application.Extensions
{
    public static class PathExtensions
    {
        public static readonly string MediaPath = "Media";
        public static readonly string Directory= Path.Combine(System.IO.Directory.GetCurrentDirectory());

        public readonly struct Product
        {
            public static readonly string Images = Path.Combine(MediaPath, "product", "images");
            public static readonly string DefaultImageName = "product.png";
        }

        public readonly struct Author
        {
            public static readonly string Images = Path.Combine(MediaPath, "author", "images");
            public static readonly string Man_DefaultImageName = "author-man.png";
            public static readonly string Woman_DefaultImageName = "author-woman.png";
        }

        public readonly struct Category
        {
            public static readonly string Images = Path.Combine(MediaPath, "category", "images");
            public static readonly string DefaultImageName = "category.png";
        }

        public readonly struct EBook
        {
            public static readonly string Files = Path.Combine(MediaPath, "ebook", "files");
        }

        public readonly struct Publisher
        {
            public static readonly string Images = Path.Combine(MediaPath, "publisher", "images");
            public static readonly string DefaultImageName = "publisher.png";
        }

        public readonly struct Translator
        {
            public static readonly string Images = Path.Combine(MediaPath, "translator", "images");
            public static readonly string Man_DefaultImageName = "translator-man.png";
            public static readonly string Woman_DefaultImageName = "translator-woman.png";
        }

        public readonly struct User
        {
            public static readonly string Images = Path.Combine(MediaPath, "user", "images");
            public static readonly string DefaultImageName = "user.png";
        }

    }
}

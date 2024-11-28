using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Book : Entity<Guid>
    {
        public int NumberOfPages { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Languages Language { get; set; }
        public string? Shabak { get; set; }
        public DateTime PublishYear { get; set; }
        public float? WeightInGram { get; set; }
        public int? Edition { get; set; }   
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }        
        public Guid ProductId { get; set; }


        public Product Product { get; set; }
        public Publisher Publisher { get; set; }
        public Translator? Translator { get; set; }
        public IList<Author> Authors { get; set; }

    }
}

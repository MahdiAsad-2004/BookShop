using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class EBook : Entity<Guid> 
    {
        public string FileName { get; set; }
        public float FileSize_KB{ get; set; }
        public int NumberOfPages { get; set; }
        public Language Language { get; set; }
        public EBookFileFormat FileFormat { get; set; }
        public DateTime PublishYear { get; set; }
        public int? Edition { get; set; }
        public Guid ProductId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid? TranslatorId { get; set; }



        public Product Product { get; set; }
        public Publisher Publisher { get; set; }
        public Translator? Translator { get; set; }
        public IList<Author_EBook> Author_EBooks { get; set; }
     
    }
}

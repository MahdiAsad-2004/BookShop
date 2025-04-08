using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            
            //relations
            builder.HasOne(a => a.Product).WithOne(a => a.Book).HasForeignKey<Book>(a => a.ProductId);
            builder.HasOne(a => a.Publisher).WithMany(a => a.Books).HasForeignKey(a => a.PublisherId);
            builder.HasOne(a => a.Translator).WithMany(a => a.Books).HasForeignKey(a => a.TranslatorId);
            builder.HasMany(a => a.Author_Books).WithOne(a => a.Book).HasForeignKey(a => a.BookId);

            //properties
            builder.Property(a => a.Shabak).HasColumnType("VarChar(20)");
        }
    }
}

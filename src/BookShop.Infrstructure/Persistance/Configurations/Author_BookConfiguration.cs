using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class Author_BookConfiguration : IEntityTypeConfiguration<Author_Book>
    {
        public void Configure(EntityTypeBuilder<Author_Book> builder)
        {
            builder.ToTable("Author_Books");

            //relations
            builder.HasOne(a => a.Book).WithMany(a => a.Author_Books).HasForeignKey(a => a.BookId);
            builder.HasOne(a => a.Author).WithMany(a => a.Author_Books).HasForeignKey(a => a.AuthorId);

        }
    }
}

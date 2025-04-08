using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class Author_EBookConfiguration : IEntityTypeConfiguration<Author_EBook>
    {
        public void Configure(EntityTypeBuilder<Author_EBook> builder)
        {
            builder.ToTable("Author_EBooks");

            //relations
            builder.HasOne(a => a.EBook).WithMany(a => a.Author_EBooks).HasForeignKey(a => a.EBookId);
            builder.HasOne(a => a.Author).WithMany(a => a.Author_EBooks).HasForeignKey(a => a.AuthorId);

        }
    }
}

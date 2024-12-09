using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");

            //relations
            builder.HasMany(a => a.Books).WithMany(a => a.Authors);
            builder.HasMany(a => a.EBooks).WithMany(a => a.Authors);

            //properties
            builder.Property(a => a.Name).HasColumnType("NVarChar(30)");
            builder.Property(a => a.ImageName).HasColumnType("VarChar(50)");


        }
    }
}

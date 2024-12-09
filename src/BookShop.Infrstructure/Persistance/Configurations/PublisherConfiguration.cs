using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publishers");

            //Relations
            builder.HasMany(p => p.Books).WithOne(a => a.Publisher).HasForeignKey(a => a.PublisherId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.EBooks).WithOne(a => a.Publisher).HasForeignKey(a => a.PublisherId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.Cascade);

            //Properties
            builder.Property(a => a.Title).HasColumnType("NVarChar(30)");
            builder.Property(a => a.ImageName).HasColumnType("VarChar(50)");

        }
    }
}

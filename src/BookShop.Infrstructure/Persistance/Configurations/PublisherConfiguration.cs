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
            builder.HasMany(p => p.Books).WithOne(a => a.Publisher).HasForeignKey(a => a.PublisherId);
            builder.HasMany(p => p.EBooks).WithOne(a => a.Publisher).HasForeignKey(a => a.PublisherId);

        }
    }
}

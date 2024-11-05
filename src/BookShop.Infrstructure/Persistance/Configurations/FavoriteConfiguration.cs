using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");
            builder.HasOne(a => a.User).WithMany(a => a.Favorites).HasForeignKey(a => a.UserId);
            builder.HasOne(a => a.Product).WithMany(a => a.Favorites).HasForeignKey(a => a.ProductId);

        }
    }
}

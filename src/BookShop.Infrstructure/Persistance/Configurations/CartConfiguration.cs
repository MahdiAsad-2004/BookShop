using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            //relations
            builder.HasOne(a => a.User).WithOne(a => a.Cart).HasForeignKey<Cart>(a => a.UserId);
            builder.HasMany(a => a.CartItems).WithOne(a => a.Cart).HasForeignKey(a => a.CartId);
        }
    }
}

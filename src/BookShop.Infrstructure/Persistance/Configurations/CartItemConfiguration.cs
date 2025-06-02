using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            //relations
            builder.HasOne(a => a.Cart).WithMany(a => a.CartItems).HasForeignKey(a => a.CartId);
            builder.HasOne(a => a.Product).WithMany(a => a.CartItems).HasForeignKey(a => a.ProductId);


        }
    }
}

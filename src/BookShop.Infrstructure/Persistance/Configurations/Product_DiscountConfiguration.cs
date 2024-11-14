using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class Product_DiscountConfiguration : IEntityTypeConfiguration<Product_Discount>
    {
        public void Configure(EntityTypeBuilder<Product_Discount> builder)
        {
            builder.ToTable("Product_Dsicounts");
            builder.HasOne(a => a.Product).WithMany(a => a.Product_Discounts).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);
            builder.HasOne(a => a.Discount).WithMany(a => a.Product_Discounts).HasForeignKey(a => a.DiscountId).HasPrincipalKey(a => a.Id);

        }
    }
}

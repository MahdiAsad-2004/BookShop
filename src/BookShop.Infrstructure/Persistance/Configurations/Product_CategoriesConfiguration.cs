using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    //public class Product_CategoryConfiguration : IEntityTypeConfiguration<Product_Category>
    //{
    //    public void Configure(EntityTypeBuilder<Product_Category> builder)
    //    {
    //        builder.ToTable("Product_Categories");

    //        builder.HasOne(a => a.Category).WithMany(a => a.Product_Categories).HasForeignKey(a => a.CategoryId).HasPrincipalKey(a => a.Id);
    //        builder.HasOne(a => a.Product).WithMany(a => a.Product_Categories).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);

    //    }
    //}
}

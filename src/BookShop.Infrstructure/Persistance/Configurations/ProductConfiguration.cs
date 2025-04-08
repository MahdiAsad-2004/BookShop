using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            
            //Relations
            builder.HasOne(a => a.Book).WithOne(a => a.Product).HasForeignKey<Book>(a => a.ProductId);
            builder.HasOne(a => a.EBook).WithOne(a => a.Product).HasForeignKey<EBook>(a => a.ProductId);
            builder.HasMany(a => a.Favorites).WithOne(a => a.Product);
            builder.HasMany(a => a.Reviews).WithOne(a => a.Product).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);
            builder.HasMany(a => a.Product_Discounts).WithOne(a => a.Product).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);
            //builder.HasMany(a => a.Product_Categories).WithOne(a => a.Product).HasForeignKey(a => a.ProductId);
            builder.HasOne(a => a.Category).WithMany(a => a.Products).HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Restrict);


            //Properties
            builder.Ignore(a => a.DiscountedPrice);
            builder.Ignore(a => a.ReviewsAcceptedAverageScore);
            builder.Property(a => a.Title).HasColumnType("NVarChar(30)");
            builder.Property(a => a.ImageName).HasColumnType("VarChar(50)");
            builder.Property(a => a.DescriptionHtml).HasColumnType("NVarChar(Max)");



        }
    }
}

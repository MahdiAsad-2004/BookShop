using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            //relations
            builder.HasMany(a => a.Childs).WithOne(a => a.Parent).HasForeignKey(a => a.ParentId);
            builder.HasMany(a => a.Product_Categories).WithOne(a => a.Category).HasForeignKey(a => a.CategoryId);

            //properties
            builder.Property(a => a.Title).HasColumnType("NVarChar(30)");
            builder.Property(a => a.ImageName).HasColumnType("VarChar(50)");

        }
    }
}

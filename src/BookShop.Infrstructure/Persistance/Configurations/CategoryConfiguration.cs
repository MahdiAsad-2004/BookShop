using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categorys");
            builder.HasMany(a => a.Childs).WithOne(a => a.Parent).HasForeignKey(a => a.ParentId);
            builder.HasMany(a => a.Products).WithMany(a => a.Categories);
            
        }
    }
}

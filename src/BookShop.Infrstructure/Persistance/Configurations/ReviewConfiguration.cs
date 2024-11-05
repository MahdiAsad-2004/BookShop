using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasOne(a => a.User).WithMany(a => a.Reviews).HasForeignKey(a => a.UserId);
            builder.HasOne(a => a.Product).WithMany(a => a.Reviews).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);


        }
    }
}

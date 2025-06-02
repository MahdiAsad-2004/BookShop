using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            //relations
            builder.HasOne(a => a.User).WithMany(a => a.Orders).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.OrderItems).WithOne(a => a.Order).HasForeignKey(a => a.OrderId);
            builder.HasMany(a => a.OrderStatuses).WithOne(a => a.Order).HasForeignKey(a => a.OrderId);

            //properties
            builder.Property(a => a.TrackingCode).HasColumnType("VarChar(50)");
            builder.Property(a => a.UsedDiscountCode).HasColumnType("VarChar(50)");
        }
    }
}

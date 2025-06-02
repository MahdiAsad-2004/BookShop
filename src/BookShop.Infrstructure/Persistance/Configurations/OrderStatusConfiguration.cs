using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatuses");

            //relations
            builder.HasOne(a => a.Order).WithMany(a => a.OrderStatuses).HasForeignKey(a => a.OrderId);

            //properties


        }
    }
}

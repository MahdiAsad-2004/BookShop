using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            //relations
            builder.HasOne(a => a.Order).WithMany(a => a.OrderItems).HasForeignKey(a => a.OrderId);
            builder.HasOne(a => a.Product).WithMany(a => a.OrderItems).HasForeignKey(a => a.ProductId);


            //properties
            builder.Property(a => a.Product_Title).HasColumnType("NVarChar(50)");


        }
    }
}

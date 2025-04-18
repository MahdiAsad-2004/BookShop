﻿using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");
            
            //relations
            builder.HasMany(a => a.Product_Discounts).WithOne(a => a.Discount).HasForeignKey(a => a.DiscountId).HasPrincipalKey(a => a.Id);

            //properties
            builder.Property(a => a.Name).HasColumnType("VarChar(30)");

        }
    }
}

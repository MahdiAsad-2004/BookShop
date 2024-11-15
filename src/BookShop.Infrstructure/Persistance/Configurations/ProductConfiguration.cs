﻿using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasOne(a => a.Book).WithOne(a => a.Product).HasForeignKey<Book>(a => a.ProductId);
            builder.HasOne(a => a.EBook).WithOne(a => a.Product).HasForeignKey<EBook>(a => a.ProductId);
            builder.HasMany(a => a.Favorites).WithOne(a => a.Product);
            builder.HasMany(a => a.Reviews).WithOne(a => a.Product).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);
            builder.HasMany(a => a.Product_Discounts).WithOne(a => a.Product).HasForeignKey(a => a.ProductId).HasPrincipalKey(a => a.Id);
            builder.HasMany(a => a.Categories).WithMany(a => a.Products);




        }
    }
}

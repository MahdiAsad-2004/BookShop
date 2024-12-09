using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class EBookConfiguration : IEntityTypeConfiguration<EBook>
    {
        public void Configure(EntityTypeBuilder<EBook> builder)
        {
            builder.ToTable("EBooks");

            //Relations
            builder.HasOne(a => a.Product).WithOne(a => a.EBook).HasForeignKey<EBook>(a => a.ProductId);
            builder.HasOne(a => a.Translator).WithMany(a => a.EBooks).HasForeignKey(a => a.TranslatorId);
            builder.HasOne(a => a.Publisher).WithMany(a => a.EBooks).HasForeignKey(a => a.PublisherId);
            builder.HasMany(a => a.Authors).WithMany(a => a.EBooks);
         
            //Properties


        }
    }
}

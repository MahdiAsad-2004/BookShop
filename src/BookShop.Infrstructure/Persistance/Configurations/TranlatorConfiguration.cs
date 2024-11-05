using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class TranslatorConfiguration : IEntityTypeConfiguration<Translator>
    {
        public void Configure(EntityTypeBuilder<Translator> builder)
        {
            builder.ToTable("Translators");
            builder.HasMany(a => a.Books).WithOne(a => a.Translator).HasForeignKey(a => a.TranslatorId);
            builder.HasMany(a => a.EBooks).WithOne(a => a.Translator).HasForeignKey(a => a.TranslatorId);

        }
    }
}

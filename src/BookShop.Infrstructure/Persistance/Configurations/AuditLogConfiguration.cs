using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");
            
            //relations
            builder.HasOne(a => a.User).WithMany(a => a.AuditLogs).HasForeignKey(a => a.UserId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.Restrict);

            //properties
            builder.Property(a => a.EntityTypeFullName).HasColumnType("VarChar(100)");


        }

    }
}

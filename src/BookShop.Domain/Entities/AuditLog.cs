
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class AuditLog : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public AuditType AuditType { get; set; }
        public string NewObject { get; set; }
        public string OldObject { get; set; }
        public string EntityTypeFullName { get; set; }
        public DateTime Date { get; set; }



        

        public User User { get; set; }


    

        public Type? GetEntityType()
        {
            return Type.GetType(EntityTypeFullName);
        }

        


    }


}

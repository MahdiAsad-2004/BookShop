
using BookShop.Domain.Entities;

namespace BookShop.Infrastructure.Persistance.SeedDatas
{
    public class UsersSeed
    {
        private static readonly Guid _superAdminId = Guid.NewGuid();
        public static readonly User SuperAdmin = new User
        {
            Id = _superAdminId,
            ConcurrencyStamp = string.Empty,
            CreateBy = _superAdminId.ToString(),
            CreateDate = DateTime.UtcNow,
            DeleteDate = null,
            DeletedBy = null,
            Email = "MohammadMahdiAsadi2004@gmail.com",
            EmailConfirmed = true,
            IsDeleted = false,
            ImageName = string.Empty,
            LastModifiedBy = _superAdminId.ToString(),
            LastModifiedDate = DateTime.UtcNow,
            Name = "Mahdi Asadi",
            PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
            //PasswordHistories = new List<PasswordHistory>
            //{
            //    new PasswordHistory
            //    {
            //        CreateBy = adminId.ToString(),
            //        CreateDate = adminCreateDate,
            //        DeleteDate = null,
            //        DeletedBy = null,
            //        Id = Guid.NewGuid(),
            //        IsDeleted = false,
            //        LastModifiedBy = adminId.ToString(),
            //        LastModifiedDate = adminCreateDate,
            //        PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
            //        UserId = adminId,
            //    },
            //},
            PhoneNumber = "09369753041",
            SecurityStamp = null,
        };


    }
}

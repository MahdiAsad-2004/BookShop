
using BookShop.Domain.Entities;

namespace BookShop.Infrastructure.Persistance.SeedDatas
{
    public class UsersSeed
    {
        private static readonly Guid _superAdminId = Guid.Parse("8826c891-6e75-48a3-b347-0d7e21113f21");
        public static readonly User SuperAdmin = new User
        {
            Id = _superAdminId,
            ConcurrencyStamp = string.Empty,
            CreateBy = _superAdminId.ToString(),
            CreateDate = DateTime.UtcNow,
            DeleteDate = null,
            DeletedBy = null,
            Email = "MohammadMahdiAsadi2004@gmail.com",
            NormalizedEmail = "MohammadMahdiAsadi2004@gmail.com",
            EmailConfirmed = true,
            IsDeleted = false,
            ImageName = string.Empty,
            LastModifiedBy = _superAdminId.ToString(),
            LastModifiedDate = DateTime.UtcNow,
            Name = "Mahdi Asadi",
            PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
            PasswordHistories = new List<PasswordHistory>
            {
                new PasswordHistory
                {
                    CreateBy = _superAdminId.ToString(),
                    CreateDate = DateTime.UtcNow,
                    DeleteDate = null,
                    DeletedBy = null,
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    LastModifiedBy = _superAdminId.ToString(),
                    LastModifiedDate = DateTime.UtcNow,
                    PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
                    UserId = _superAdminId,
                },
            },
            PhoneNumber = "09369753041",
            SecurityStamp = null,
            AccessFailedCount = 0,
            LockoutEnabled = false,
            LockoutEnd = null,
            Username = "Mahdi_Asadi",
            NormalizedUsername = "Mahdi_Asadi",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
        };


    }
}

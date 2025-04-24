using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.IntegrationTest.Common
{
    internal class TestCurrentUser : ICurrentUser
    {
        private readonly CurrentUserCacheObject _currentUserCacheObject;

        public static readonly string CurrentUserCachKey = "33f66e0c-e0bd-4847-9e12-c49baf1dbe87";
        public static readonly Guid CurrentUserId = Guid.Parse("f900a493-107e-4704-8672-b43b97eff955");
        public static readonly User User = new User
        {
            Id = CurrentUserId,
            ConcurrencyStamp = string.Empty,
            CreateBy = CurrentUserId.ToString(),
            CreateDate = DateTime.UtcNow.AddYears(5),
            DeleteDate = null,
            DeletedBy = null,
            Email = "MohammadMahdiAsadi2004@gmail.com",
            NormalizedEmail = "MohammadMahdiAsadi2004@gmail.com",
            EmailConfirmed = true,
            IsDeleted = false,
            ImageName = string.Empty,
            LastModifiedBy = CurrentUserId.ToString(),
            LastModifiedDate = DateTime.UtcNow,
            Name = "Mahdi Asadi",
            PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
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

        public TestCurrentUser(IMemoryCache memoryCache)
        {
            _currentUserCacheObject = memoryCache.Get<CurrentUserCacheObject>(CurrentUserCachKey) ?? new CurrentUserCacheObject();
        }

        public Guid? Id
        {
            get { return _currentUserCacheObject.Id; }
            init { Id = Guid.NewGuid(); }
        }
        public bool Authenticated
        {
            get { return _currentUserCacheObject.Authenticated; }
            init { Authenticated = true; }
        }
        public string Email
        {
            get { return _currentUserCacheObject.Email; }
            init { Email = value; }
        }
        public string? Name
        {
            get { return _currentUserCacheObject.Name; }
            init { Name = value; }
        }
        public string? Username
        {
            get { return _currentUserCacheObject.Username; }
            init { Username = value; }
        }
        public string? PhoneNumber
        {
            get { return _currentUserCacheObject.PhoneNumber; }
            init { PhoneNumber = value; }
        }
    }

    


    public class CurrentUserCacheObject
    {
        public Guid? Id { get; set; } = null;
        public bool Authenticated { get; set; } = false;
        public string? Email { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Username { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
    }

}

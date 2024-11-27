
using BookShop.Domain.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.IntegrationTest.Application.Common
{
    internal class TestCurrentUser : ICurrentUser
    {
        private readonly CurrentUserCacheObject _currentUserCacheObject;
        public static readonly string CurrentUserCachKey = "33f66e0c-e0bd-4847-9e12-c49baf1dbe87";
        public static readonly Guid CurrentUserId = Guid.Parse("f900a493-107e-4704-8672-b43b97eff955");
        public TestCurrentUser(IMemoryCache memoryCache)
        {
            _currentUserCacheObject = memoryCache.Get<CurrentUserCacheObject>(CurrentUserCachKey) ?? new CurrentUserCacheObject();
        }


        public Guid? Id 
        { 
            get { return _currentUserCacheObject.Id; }
            init {Id = Guid.NewGuid(); } 
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

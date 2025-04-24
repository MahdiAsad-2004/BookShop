
namespace BookShop.Domain.Constants
{
    public static class CacheConstants
    {
        public static string UserPermissions(Guid userId) => $"permissions-{userId}";


    }
}

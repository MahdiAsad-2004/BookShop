
namespace BookShop.Application.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(Guid? guid) 
        {
            return guid == null || guid.Value == Guid.Empty;
        }


    }
}


namespace BookShop.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string[] GetNames<TEnum>() where TEnum : Enum
        {
            return typeof(TEnum).GetEnumNames();
        } 


    }
}

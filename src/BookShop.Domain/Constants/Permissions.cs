
namespace BookShop.Domain.Constants
{
    public static class Permissions
    {
        public const string GetAuditLogs = "GetAuditLogsPermission";
        
        public const string AddUser = "AddUserPermission";

        public const string GetUsers = "GetUsersPermission";

        public const string AddBook = "AddBookPermission";




        public static IEnumerable<string> GetAll()
        {
            yield return GetAuditLogs;
            yield return AddUser;
            yield return GetUsers;
        }

    }
}

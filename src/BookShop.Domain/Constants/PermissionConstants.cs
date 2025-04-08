
namespace BookShop.Domain.Constants
{
    public static class PermissionConstants
    {
        //public static readonly PermissionConst GetAuditLogs = new PermissionConst("GetAuditLogsPermission");
        
        public const string GetAuditLogs = "Get_AuditLogs_Permission";
        
        public const string AddUser = "Add_User_Permission";
        public const string GetUsers = "Get_Users_Permission";

        public const string AddAuthor = "Add_Author_Permission";
        public const string UpdateAuthor = "Update_Author_Permission";
        
        public const string AddCategory = "Add_Category_Permission";
        public const string UpdateCategory = "Update_Category_Permission";

        public const string AddBook = "Add_Book_Permission";




        public static IEnumerable<string> GetAll()
        {
            yield return GetAuditLogs;
            yield return AddUser;
            yield return GetUsers;
            yield return AddAuthor;
            yield return UpdateAuthor;
            yield return AddBook;
            yield return AddCategory;
            yield return UpdateCategory;
        }
    }



    //public record struct PermissionConst(string PerrmissionName)
    //{

    //}



}

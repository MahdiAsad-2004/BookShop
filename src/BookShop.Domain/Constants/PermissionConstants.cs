
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
        
        public const string AddBook = "Add_Book_Permission";
        public const string UpdateBook = "Update_Book_Permission";
        
        public const string AddCategory = "Add_Category_Permission";
        public const string UpdateCategory = "Update_Category_Permission";

        public const string AddDiscount= "Add_Book_Permission";
        public const string UpdateDiscount= "Update_Book_Permission";
        public const string DeleteDiscount= "Delete_Book_Permission";

        public const string AddEBook = "Add_EBook_Permission";
        public const string UpdateEBook = "Update_EBook_Permission";



        public static IEnumerable<string> GetAll()
        {
            yield return GetAuditLogs;
            yield return AddUser;
            yield return GetUsers;
            yield return AddAuthor;
            yield return UpdateAuthor;
            yield return AddBook;
            yield return UpdateBook;
            yield return AddCategory;
            yield return UpdateCategory;
            yield return AddDiscount;
            yield return UpdateDiscount;
            yield return DeleteDiscount;
            yield return AddEBook;
            yield return UpdateEBook;
        }
    }



    //public record struct PermissionConst(string PerrmissionName)
    //{

    //}



}

using System.Reflection;

namespace BookShop.Domain.Constants
{
    public static class PermissionConstants
    {
        public struct AuditLog
        {
            public const string Get = "auditLog:get";
        }

        public struct User
        {
            public const string Add = "user:add";
            public const string Get = "user:get";
        }

        public struct Author
        {
            public const string Add = "author:add";
            public const string Update = "author:update";
        }

        public struct Book
        {
            public const string Add = "book:add";
            public const string Update = "book:update";
        }

        public struct Categoory
        {
            public const string Add = "category:add";
            public const string Update = "category:update";
        }

        public struct Discount
        {
            public const string Add = "discount:add";
            public const string Update = "discount:update";
            public const string Delete = "discount:delete";
        }

        public struct EBook
        {
            public const string Add = "ebook:add";
            public const string Update = "ebook:update";
        }
        
        public struct Publisher
        {
            public const string Add = "publisher:add";
            public const string Update = "publisher:update";
        }
        
        public struct Translator
        {
            public const string Add = "translator:add";
            public const string Update = "translator:update";
        }


        public static IEnumerable<string> GetAll()
        {
            return typeof(PermissionConstants)
           .GetNestedTypes()
           .SelectMany(t => t
               .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(f => f.IsLiteral && !f.IsInitOnly)
               .Select(f => f.GetRawConstantValue()?.ToString())
           )
           .Where(p => !string.IsNullOrEmpty(p))!;
        }



    }



    //public record struct PermissionConst(string PerrmissionName)
    //{

    //}



}

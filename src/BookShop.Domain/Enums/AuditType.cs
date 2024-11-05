
using System.ComponentModel;

namespace BookShop.Domain.Enums
{
    public enum AuditType
    {
        [Description("ایجاد")]
        Create,

        [Description("ویرایش")]
        Update,

        [Description("حذف")]
        Delete,

        

    }
}

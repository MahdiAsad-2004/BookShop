
using System.ComponentModel;

namespace BookShop.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("ثبت شد")]
        Submitted,

        [Description("پردازش سفارش")]
        Processing,

        [Description("بسته بندی")]
        Packing,
        
        [Description("ارسال")]
        Sending,

        [Description("تحویل داده شد")]
        Delivered,

    }
}

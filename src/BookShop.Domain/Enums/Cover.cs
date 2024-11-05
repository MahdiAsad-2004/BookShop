
using System.ComponentModel;

namespace BookShop.Domain.Enums
{
    public enum Cover
    {
        [Description("شومیز")]
        PaperbackCase,

        [Description("کاغذی")]
        PaperCover,

        [Description("گالینگور")]
        HardCoverCase,

        [Description("جلد سخت")]
        HardCover,

        [Description("چرم")]
        LeatherCover,

        [Description("پارچه ای")]
        ClothCover,



        

    }
}

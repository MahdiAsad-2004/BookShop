
using System.ComponentModel;

namespace BookShop.Domain.Enums
{
    public enum EBookFileFormat
    {
        [Description("PDF")]
        Pdf,

        [Description("EPUB")]
        Epub,
        
        [Description("MOBI")]
        Mobi,
        
        [Description("DOC")]
        Doc,

        [Description("AZW3")]
        Azw3,

        [Description("DJVU")]
        DjVu

    }
}

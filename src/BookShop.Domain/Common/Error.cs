using BookShop.Domain.Exceptions;

namespace BookShop.Domain.Common
{
    public sealed record Error(ErrorCode Code, string Description,List<ValidationError>?  ValidationErrors)
    {

        public static readonly Error None = new(ErrorCode.None, string.Empty,new List<ValidationError>());
    
   
    }



    public enum ErrorCode
    {
        None,   
        Validation,
        Authentication
    }


}

namespace BookShop.Domain.Common.Exception
{
    public class ApplicationException : System.Exception
    {
        public ApplicationException(string errorMessage) : base(errorMessage)
        {
            
        }

    }
}

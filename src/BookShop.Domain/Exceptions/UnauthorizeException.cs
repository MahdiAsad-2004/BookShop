
namespace BookShop.Domain.Exceptions
{
    public class UnauthorizeException : ApplicationException
    {
        public UnauthorizeException(string message) : base(message)
        {
            
        }
    }
}

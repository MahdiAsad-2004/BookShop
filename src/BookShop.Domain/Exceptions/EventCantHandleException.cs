
namespace BookShop.Domain.Exceptions
{
    public class EventCantHandleException : ApplicationException
    {
        public EventCantHandleException(string message) : base(message)
        {
            
        }
    }
}


using BookShop.Domain.Exceptions;

namespace BookShop.Application.Common.Response
{
    public abstract class Response
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public List<ValidationError> ValidationErrors { get; init; }
    
    
    }
}

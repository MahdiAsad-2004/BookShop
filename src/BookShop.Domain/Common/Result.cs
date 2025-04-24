
namespace BookShop.Domain.Common
{
    public abstract class Result
    {
        public object? ResultData { get; init; }
        public bool IsSuccess { get; init; }
        public Error? Error { get; init; }
    }

    public class Result<TResult> : Result
    {
        public Result()
        {
               
        }
        public Result(TResult? data,bool isSuccess,Error? error = null) 
        { 
            Data = data;
            IsSuccess = isSuccess;
            Error = error;
        }
        public TResult? Data { get; init; }


       
    }

}

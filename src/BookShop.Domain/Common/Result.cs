
using System.Data.SqlTypes;

namespace BookShop.Domain.Common
{
    public abstract class Result
    {
        public object? ResultData { get; init; }
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public Error? Error { get; init; }

        public static Result<Empty> Success(string? messag = null)
        {
            return new Result<Empty>(Empty.New, true , messag);
        }

        public static Result<TResult> Success<TResult>(TResult? result, string? messag = null)
        {
            return new Result<TResult>(result, true);
        }

        public static Result<Empty> Fail(string? messag = null)
        {
            return new Result<Empty>(Empty.New, false , messag);
        }

        public static Result<TResult> Fail<TResult>(TResult? result, string? messag = null)
        {
            return new Result<TResult>(result, false);
        }
    }

    public class Result<TResult> : Result
        //where TResult : struct
    {
        public Result()
        {
               
        }
        public Result(TResult? data,bool isSuccess,string? message = null,Error? error = null) 
        { 
            Data = data;
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
            
        }
        public TResult? Data { get; init; }


     


    }

}

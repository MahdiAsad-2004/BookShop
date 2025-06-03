
using BookShop.Domain.Enums;

namespace BookShop.Domain.Common
{
    public sealed class Error
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public List<ErrorDetail> Details { get; set; }
        public int StatusCode { get; private set; } = 0;
        public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;

        public Error(ErrorCode code, string message, List<ErrorDetail>? details = null )
        {
            Code = code;
            Message = message;
            Details = details ?? new List<ErrorDetail>();
        }

        public void SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
        }

        public static readonly Error None = new(ErrorCode.None, string.Empty, new List<ErrorDetail>());

    }


    //public sealed class ErrorDetail
    //{
    //    public ErrorCode Code { get; }
    //    public string Target { get; }
    //    public string Message { get; }
    //    public ErrorDetail(ErrorCode code, string target, string message)
    //    {

    //    }

    //}

    public sealed record ErrorDetail(ErrorCode Code, string Target,string Message);



}

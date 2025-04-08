using System.Net;

namespace WebShoppingAPI.ExceptionHandling.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public BaseException(string  message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

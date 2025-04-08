using System.Net;

namespace WebShoppingAPI.ExceptionHandling.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException() : base("Forbidden", HttpStatusCode.Forbidden)
        {

        }
    }
}

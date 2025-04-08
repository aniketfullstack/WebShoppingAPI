using System.Net;

namespace WebShoppingAPI.ExceptionHandling.Exceptions
{
    public class NotFoundException:BaseException
    {
        public NotFoundException():base("Not Found", HttpStatusCode.NotFound)
        {

        }
    }
}

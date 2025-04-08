using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebShoppingAPI.ExceptionHandling.Exceptions;

namespace WebShoppingAPI.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();

            problemDetails.Instance = httpContext.Request.Path;
            if (exception is BaseException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                problemDetails.Title = e.Message;
            }
            else
            {
                problemDetails.Title = exception.Message;
            }

            //Log exception
            logger.LogError(exception, "An unhandled exception occurred.");

            problemDetails.Status = httpContext.Response.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

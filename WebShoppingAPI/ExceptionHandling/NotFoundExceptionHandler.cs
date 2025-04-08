using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebShoppingAPI.Errors;
using WebShoppingAPI.ExceptionHandling.Exceptions;

namespace WebShoppingAPI.Exceptions
{
    public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
        {
            if(exception is not NotFoundException ex)
            {
                return false;
            }

            //handle error 

            var problemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            logger.LogError(exception, "An unhandled exception occurred.");


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

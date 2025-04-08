using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebShoppingAPI.Errors;
using WebShoppingAPI.ExceptionHandling.Exceptions;

namespace WebShoppingAPI.Exceptions
{
    public class ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
        {
            if(exception is not ForbiddenException ex)
            {
                return false;
            }

            //handle error 

            var problemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "You Don't have access",
                Status = StatusCodes.Status403Forbidden,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            logger.LogError(exception, "An unhandled exception occurred.");


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

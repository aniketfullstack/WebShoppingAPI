using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebShoppingAPI.Errors;

namespace WebShoppingAPI.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Log exception
            logger.LogError(exception, "An unhandled exception occurred.");

            var errorResponse = new ErrorResponse
            {
                Title = "An error occurred",
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message
            };


            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}

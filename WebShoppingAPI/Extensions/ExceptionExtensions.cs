using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using WebShoppingAPI.Errors;
using WebShoppingAPI.ExceptionHandling.Exceptions;
using WebShoppingAPI.Exceptions;

namespace WebShoppingAPI.Extensions
{
    public static class ExceptionExtensions
    {
        public static void AddExceptionHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<ForbiddenExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using TaskManager.Application.Exceptions;

namespace TaskManager.API.Extensions
{
    public static class ErrorHandlingExtensions
    {
        private static IHttpContextAccessor? _contextAccessor;

        public static void ExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context => {

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        // Check if the exception is an ApiException
                        if (contextFeature.Error is ApiException apiException)
                        {
                            // Log the error using the provided logger
                            logger.LogError($"API Error occurred: {apiException}");

                            // Use the status code from the ApiException
                            context.Response.StatusCode = (int)apiException.StatusCode;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = (int)apiException.StatusCode,
                                Message = apiException.Message
                            }.ToString());
                        }
                        else
                        {
                            // Handle non-ApiException errors
                            logger.LogError($"Something went wrong: {contextFeature.Error}");

                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error. Please check the application logs for details."
                            }.ToString());
                        }
                    }
                });
            });
        }
    }
}

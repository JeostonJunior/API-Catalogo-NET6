using ApiCatalago.Models;
using Microsoft.AspNetCore.Diagnostics;
using Nancy;

namespace ApiCatalago.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        private const string CONTENT_TYPE = "application/json";
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = CONTENT_TYPE;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        }.ToString());

                    }
                });

            });

        }
    }
}

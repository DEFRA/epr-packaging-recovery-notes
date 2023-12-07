using Newtonsoft.Json;
using System.Net;

namespace EPRN.Waste.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
                _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(context, ex);
            }
        }

        private static Task ExceptionHandler(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new {
                error = new
                {
                    message = "An error occured",
                    details = ex.Message
                }
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}

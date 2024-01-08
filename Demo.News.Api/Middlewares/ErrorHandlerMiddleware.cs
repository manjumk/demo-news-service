using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Demo.News.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            /*TODO- can be logged*/
            //_logger.LogError(ex, ex.Message);
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";
            var responseModel = new ProblemDetails
            {
                Title = "An Exception Occurred",
                Status = HttpStatusCode.InternalServerError.GetHashCode(),
                Detail = "Internal Error Occurred!"
            };
            
            return response.WriteAsync(JsonSerializer.Serialize(responseModel));
        }
    }
}

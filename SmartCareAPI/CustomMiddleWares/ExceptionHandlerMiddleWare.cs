using LinkO.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace SmartCareAPI.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next , ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                HandleNotFound(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, $"Something went Wrong");
            var Problem = new ProblemDetails()
            {
                Title = "An Unexpected error Occurred",
                Detail = ex.Message,
                Instance = context.Request.Path,
                Status = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            };
            context.Response.StatusCode = Problem.Status.Value;
            await context.Response.WriteAsJsonAsync(Problem);
        }

        private static void HandleNotFound(HttpContext context)
        {
            // 404 NotFound
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resource You are looking for is not found",
                    Instance = context.Request.Path
                };
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.CustomMiddleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something went wrong. Please try again later.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var problem = new ProblemDetails()
                {
                    Title = "An unexpected error occured",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

               await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}

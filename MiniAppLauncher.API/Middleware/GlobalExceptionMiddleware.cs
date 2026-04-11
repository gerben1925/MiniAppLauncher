using Microsoft.Data.SqlClient;

namespace MiniAppLauncher.API.Middleware
{
  
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Database error occurred.");
                await WriteErrorResponse(context, 500, "A database error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception.");
                await WriteErrorResponse(context, 500, "An unexpected error occurred.");
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                //StatusCode = statusCode,
                //Message = message
                message = new
                {
                    statusCode = statusCode,
                    status = message,
                }
            });
        }
    }
}

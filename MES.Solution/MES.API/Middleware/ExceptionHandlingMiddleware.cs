using MES.Domain.Exceptions;
using System.Text.Json;

namespace MES.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate request, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await WriteErrorAsync(context, ex.Message);
            }
            catch (DomainException ex)
            {
                logger.LogWarning(ex, "Domain rule violated: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await WriteErrorAsync(context, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await WriteErrorAsync(context, "Unauthorized.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteErrorAsync(context, "An unexpected error occurred.");
            }
        }

        private async Task WriteErrorAsync(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse(
                context.Response.StatusCode,
                message,
                DateTime.UtcNow);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
    public record ErrorResponse(
        int StatusCode,
        string Message,
        DateTime OccurredAt);
}

using System.Net;
using System.Text.Json;
using StockPortfolioAPI.Models.Exceptions;

namespace StockPortfolioAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = new
                {
                    message = "An error occurred while processing your request",
                    details = exception.Message,
                    timestamp = DateTime.UtcNow
                }
            };

            switch (exception)
            {
                case EntityNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new
                    {
                        error = new
                        {
                            message = exception.Message,
                            details = exception.Message,
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                case DuplicateEntityException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response = new
                    {
                        error = new
                        {
                            message = exception.Message,
                            details = exception.Message,
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                case InsufficientQuantityException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = exception.Message,
                            details = exception.Message,
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                case ArgumentException:
                case InvalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = exception.Message,
                            details = exception.Message,
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

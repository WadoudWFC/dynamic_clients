using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain.Shared;

namespace MultipleHttpClient.Application.Commons.Behavior
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        // SECURITY: Define safe error messages that don't reveal system information
        private static readonly Dictionary<Type, string> SafeErrorMessages = new()
        {
            { typeof(UnauthorizedAccessException), "Access denied. Please check your permissions." },
            { typeof(KeyNotFoundException), "The requested resource was not found." },
            { typeof(ArgumentNullException), "Invalid request. Please check your input." },
            { typeof(ArgumentException), "Invalid request parameters." },
            { typeof(InvalidOperationException), "Unable to process request at this time." },
            { typeof(TimeoutException), "Request timed out. Please try again later." },
            { typeof(NotSupportedException), "Operation not supported." }
        };

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // SECURITY: Log full details server-side but sanitize client response
                var correlationId = Guid.NewGuid().ToString("N")[..8];
                _logger.LogError(ex, "Unhandled exception {0}: {1}", correlationId, ex.Message);

                await HandleExceptionAsync(context, ex, correlationId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow
            };

            // SECURITY: Categorize exceptions and provide safe responses
            switch (exception)
            {
                case UnauthorizedAccessException:
                    response.Code = "UNAUTHORIZED";
                    response.Message = SafeErrorMessages[typeof(UnauthorizedAccessException)];
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    response.Code = "NOT_FOUND";
                    response.Message = SafeErrorMessages[typeof(KeyNotFoundException)];
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ArgumentNullException:
                case ArgumentException:
                    response.Code = "BAD_REQUEST";
                    response.Message = SafeErrorMessages[typeof(ArgumentException)];
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException when exception.Message.Contains("authentication"):
                    response.Code = "AUTH_ERROR";
                    response.Message = "Authentication configuration error. Please contact support.";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                case TimeoutException:
                    response.Code = "TIMEOUT";
                    response.Message = SafeErrorMessages[typeof(TimeoutException)];
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    break;

                // SECURITY: Catch-all for any other exception
                default:
                    response.Code = "INTERNAL_ERROR";
                    response.Message = "An unexpected error occurred. Please contact support if the problem persists.";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // SECURITY: Only add technical details in development
            if (_environment.IsDevelopment())
            {
                response.Details = exception.Message;
                response.StackTrace = exception.StackTrace;
                response.InnerException = exception.InnerException?.Message;
            }
            else
            {
                // PRODUCTION: Never expose technical details
                response.Details = null;
                response.StackTrace = null;
                response.InnerException = null;

                // Only provide correlation ID for support
                response.SupportMessage = $"Please provide this ID when contacting support: {correlationId}";
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
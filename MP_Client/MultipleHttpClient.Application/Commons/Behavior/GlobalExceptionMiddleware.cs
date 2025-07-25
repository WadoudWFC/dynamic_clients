using System.ComponentModel.DataAnnotations;
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
        private readonly IInputSanitizationService _sanitizationService;

        private static readonly Dictionary<Type, string> SafeErrorMessages = new()
        {
            { typeof(UnauthorizedAccessException), "Access denied. Please check your permissions." },
            { typeof(KeyNotFoundException), "The requested resource was not found." },
            { typeof(ArgumentNullException), "Invalid request. Please check your input." },
            { typeof(ArgumentException), "Invalid request parameters." },
            { typeof(InvalidOperationException), "Unable to process request at this time." },
            { typeof(TimeoutException), "Request timed out. Please try again later." },
            { typeof(NotSupportedException), "Operation not supported." },
            { typeof(ValidationException), "Validation failed. Please check your input." }
        };

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment, IInputSanitizationService sanitizationService)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
            _sanitizationService = sanitizationService;
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
                _logger.LogError(ex, "Unhandled exception {CorrelationId}: {Message} | Path: {Path} | User: {User} | IP: {IP}",
                    correlationId, ex.Message, context.Request.Path,
                    context.User?.Identity?.Name ?? "Anonymous",
                    context.Connection.RemoteIpAddress);

                await HandleExceptionAsync(context, ex, correlationId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
        {
            // context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    response.Code = "VALIDATION_ERROR";
                    response.Message = SafeErrorMessages[typeof(ValidationException)];
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

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

                    // SECURITY: Sanitize parameter names in development
                    if (_environment.IsDevelopment() && exception is ArgumentException argEx)
                    {
                        response.Details = _sanitizationService.SanitizeHtml($"Parameter: {argEx.ParamName}");
                    }
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

                case NotSupportedException:
                    response.Code = "NOT_SUPPORTED";
                    response.Message = SafeErrorMessages[typeof(NotSupportedException)];
                    context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    break;

                case TaskCanceledException:
                case OperationCanceledException:
                    response.Code = "REQUEST_CANCELLED";
                    response.Message = "Request was cancelled or timed out.";
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    break;

                case HttpRequestException httpEx:
                    response.Code = "EXTERNAL_SERVICE_ERROR";
                    response.Message = "External service unavailable. Please try again later.";
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;

                    _logger.LogWarning("External service error: {Message}", httpEx.Message);
                    break;

                case JsonException:
                    response.Code = "INVALID_JSON";
                    response.Message = "Invalid request format. Please check your request data.";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    response.Code = "INTERNAL_ERROR";
                    response.Message = "An unexpected error occurred. Please contact support if the problem persists.";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    _logger.LogCritical(exception, "Unhandled exception type: {ExceptionType}", exception.GetType().Name);
                    break;
            }

            if (_environment.IsDevelopment())
            {
                response.Details = response.Details ?? _sanitizationService.SanitizeHtml(exception.Message);
                response.StackTrace = _sanitizationService.SanitizeHtml(exception.StackTrace ?? "");
                response.InnerException = exception.InnerException != null
                    ? _sanitizationService.SanitizeHtml(exception.InnerException.Message)
                    : null;
            }
            else
            {
                response.Details = null;
                response.StackTrace = null;
                response.InnerException = null;

                response.SupportMessage = $"Please provide this ID when contacting support: {correlationId}";
            }

            object? requestInfo = null;
            if (_environment.IsDevelopment())
            {
                requestInfo = new
                {
                    Path = _sanitizationService.SanitizeHtml(context.Request.Path.Value ?? ""),
                    Method = context.Request.Method,
                    UserAgent = _sanitizationService.SanitizeHtml(context.Request.Headers["User-Agent"].ToString()),
                    ContentType = _sanitizationService.SanitizeHtml(context.Request.ContentType ?? ""),
                    QueryString = _sanitizationService.SanitizeHtml(context.Request.QueryString.Value ?? "")
                };
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment(),
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var finalResponse = _environment.IsDevelopment() && requestInfo != null
                ? new { response, requestInfo }
                : (object)response;

            var jsonResponse = JsonSerializer.Serialize(finalResponse, jsonOptions);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
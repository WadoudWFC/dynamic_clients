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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case UnauthorizedAccessException:
                    response.Code = "UNAUTHORIZED";
                    response.Message = "You are not authorized to access this resource";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    response.Code = "NOT_FOUND";
                    response.Message = "The requested resource was not found";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ArgumentNullException:
                    response.Code = "BAD_REQUEST";
                    response.Message = "Invalid request parameters";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException ioe when ioe.Message.Contains("authentication handler"):
                    response.Code = "AUTH_CONFIG_ERROR";
                    response.Message = "Authentication configuration error";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    response.Code = "INTERNAL_ERROR";
                    response.Message = "An error occurred while processing your request";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Add details only in development
            if (_environment.IsDevelopment())
            {
                response.Details = exception.Message;
                response.StackTrace = exception.StackTrace;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
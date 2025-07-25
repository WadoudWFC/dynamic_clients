using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class XssProtectionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IInputSanitizationService _sanitizationService;
    private readonly ILogger<XssProtectionBehavior<TRequest, TResponse>> _logger;

    public XssProtectionBehavior(IInputSanitizationService sanitizationService, ILogger<XssProtectionBehavior<TRequest, TResponse>> logger)
    {
        _sanitizationService = sanitizationService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        SanitizeRequest(request);
        var response = await next();
        SanitizeResponse(response);
        return response;
    }
    private void SanitizeRequest(TRequest request)
    {
        if (request == null) return;

        var properties = request.GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

        foreach (var property in properties)
        {
            var value = property.GetValue(request) as string;
            if (!string.IsNullOrEmpty(value))
            {
                if (_sanitizationService.ContainsMaliciousContent(value))
                {
                    _logger.LogWarning("Potential XSS content detected in {RequestType}.{PropertyName}",
                        typeof(TRequest).Name, property.Name);
                }

                var sanitizedValue = property.Name.ToLower().Contains("email")
                    ? _sanitizationService.SanitizeForJson(value)
                    : _sanitizationService.SanitizeHtml(value);

                property.SetValue(request, sanitizedValue);
            }
        }
    }
    private void SanitizeResponse(TResponse response)
    {
        if (response == null) return;

        if (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueProperty = response.GetType().GetProperty("Value");
            var value = valueProperty?.GetValue(response);
            if (value != null)
            {
                SanitizeObject(value);
            }
        }
        else
        {
            SanitizeObject(response);
        }
    }
    private void SanitizeObject(object obj)
    {
        if (obj == null) return;
        var properties = obj.GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);
        foreach (var property in properties)
        {
            var value = property.GetValue(obj) as string;
            if (!string.IsNullOrEmpty(value))
            {
                var sanitizedValue = _sanitizationService.SanitizeHtml(value);
                property.SetValue(obj, sanitizedValue);
            }
        }
    }
}
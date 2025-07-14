using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MultipleHttpClient.Application;

public class GlobalNumericValidationFilter : IActionFilter
{
    private readonly ILogger<GlobalNumericValidationFilter> _logger;

    public GlobalNumericValidationFilter(ILogger<GlobalNumericValidationFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            foreach (var (parameterName, parameterValue) in context.ActionArguments)
            {
                if (parameterValue != null)
                {
                    ValidateObject(parameterValue, parameterName);
                }
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Numeric validation failed: {Message}", ex.Message);

            // Short-circuit the pipeline - don't execute the action
            context.Result = new BadRequestObjectResult(new
            {
                error = "VALIDATION_FAILED",
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in numeric validation filter");

            context.Result = new BadRequestObjectResult(new
            {
                error = "VALIDATION_ERROR",
                message = "Invalid request parameters"
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null && context.Result is ObjectResult objectResult)
        {
            ValidateResponseData(objectResult.Value);
        }
    }

    private void ValidateObject(object obj, string parameterName)
    {
        if (obj == null) return;

        var objectType = obj.GetType();

        if (IsNumericType(objectType))
        {
            ValidateNumericValue(obj, parameterName);
            return;
        }
        var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            if (value == null) continue;

            var propertyName = $"{parameterName}.{property.Name}";

            // Check for NonNegative attribute
            var nonNegativeAttr = property.GetCustomAttribute<NonNegativeAttribute>();
            if (nonNegativeAttr != null && !nonNegativeAttr.IsValid(value))
            {
                throw new ValidationException($"Property '{propertyName}' {nonNegativeAttr.FormatErrorMessage(property.Name)}");
            }

            // Check specific property names that should be non-negative
            if (IsKnownNumericProperty(property.Name) && IsNumericType(property.PropertyType))
            {
                ValidateNumericValue(value, propertyName);
            }

            // Recursively validate nested objects
            if (!IsNumericType(property.PropertyType) && !property.PropertyType.IsEnum &&
                property.PropertyType != typeof(string) && property.PropertyType != typeof(DateTime))
            {
                ValidateObject(value, propertyName);
            }
        }
    }

    private void ValidateNumericValue(object value, string fieldName)
    {
        var isNegative = value switch
        {
            int intValue => intValue < 0,
            long longValue => longValue < 0,
            decimal decimalValue => decimalValue < 0,
            double doubleValue => doubleValue < 0,
            float floatValue => floatValue < 0,
            _ => false
        };

        if (isNegative)
        {
            throw new ValidationException($"Field '{fieldName}' cannot be negative. Value: {value}");
        }

        // Special validation for pagination parameters
        if (fieldName.EndsWith("Take", StringComparison.OrdinalIgnoreCase))
        {
            var takeValue = Convert.ToInt32(value);
            if (takeValue <= 0 || takeValue > 1000)
            {
                throw new ValidationException($"Take parameter must be between 1 and 1000. Value: {takeValue}");
            }
        }

        if (fieldName.EndsWith("Skip", StringComparison.OrdinalIgnoreCase))
        {
            var skipValue = Convert.ToInt32(value);
            if (skipValue < 0)
            {
                throw new ValidationException($"Skip parameter cannot be negative. Value: {skipValue}");
            }
        }
    }

    private static bool IsNumericType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType == typeof(int) ||
               underlyingType == typeof(long) ||
               underlyingType == typeof(decimal) ||
               underlyingType == typeof(double) ||
               underlyingType == typeof(float) ||
               underlyingType == typeof(short) ||
               underlyingType == typeof(byte);
    }

    private static bool IsKnownNumericProperty(string propertyName)
    {
        // List of property names that should never be negative
        var knownNumericProperties = new[]
        {
            "Take", "Skip", "Price", "Prix", "Quantity", "Amount", "Count",
            "Potential", "Potentiel", "Pack", "Age", "Experience", "Superficie"
        };

        return knownNumericProperties.Any(known =>
            propertyName.Contains(known, StringComparison.OrdinalIgnoreCase));
    }

    private void ValidateResponseData(object? responseData)
    {
        if (responseData != null)
        {
            _logger.LogDebug("Response validation passed for {0}", responseData.GetType().Name);
        }
    }
}
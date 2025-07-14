using System.ComponentModel.DataAnnotations;

namespace MultipleHttpClient.Application;

[AttributeUsage(AttributeTargets.Property)]
public class NonNegativeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        return value switch
        {
            int intValue => intValue >= 0,
            decimal decimalValue => decimalValue >= 0,
            double doubleValue => doubleValue >= 0,
            float floatValue => floatValue >= 0,
            _ => true
        };
    }
    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be non-negative";
    }
}

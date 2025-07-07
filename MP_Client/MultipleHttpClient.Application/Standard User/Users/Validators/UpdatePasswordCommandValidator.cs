using FluentValidation;

namespace MultipleHttpClient.Application.Users.Validators;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(12) // SECURITY: Increased from 8 to 12
            .WithMessage("Password must be at least 12 characters long")
            .MaximumLength(128)
            .WithMessage("Password must be less than 128 characters")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number")
            .Matches(@"[!@#$%^&*(),.?""':{}|<>+=\-_~`\[\]\\\/;]") // SECURITY: Fixed regex
            .WithMessage("Password must contain at least one special character (!@#$%^&*(),.?\":{}|<>+=\\-_~`[]\\\\\\//;)")
            .Must(NotContainCommonPatterns)
            .WithMessage("Password cannot contain common patterns (123, abc, qwerty, etc.)");
    }

    private static bool NotContainCommonPatterns(string password)
    {
        if (string.IsNullOrEmpty(password)) return true;

        var commonPatterns = new[]
        {
            "123456", "password", "123456789", "12345678", "12345", "1234567",
            "1234567890", "qwerty", "abc123", "111111", "123123", "admin",
            "letmein", "welcome", "monkey", "dragon", "master", "computer",
            "abcdef", "qwertz", "azerty", "654321", "987654321"
        };

        var lowerPassword = password.ToLowerInvariant();

        // Check for exact matches
        if (commonPatterns.Any(pattern => lowerPassword.Contains(pattern)))
            return false;

        // Check for sequential patterns
        if (HasSequentialChars(lowerPassword, 4))
            return false;

        // Check for repeated patterns
        if (HasRepeatedPattern(lowerPassword, 3))
            return false;

        return true;
    }
    private static bool HasSequentialChars(string password, int minLength)
    {
        if (string.IsNullOrEmpty(password)) return false;

        for (int i = 0; i < password.Length - minLength + 1; i++)
        {
            // Check for sequential characters (e.g., "abcd", "1234")
            if (IsSequential(password.Substring(i, minLength)))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsSequential(string str)
    {
        // Check if the string is sequential in terms of ASCII values
        for (int i = 1; i < str.Length; i++)
        {
            if (str[i] != str[i - 1] + 1)
            {
                return false;
            }
        }
        return true;
    }

    private static bool HasRepeatedPattern(string password, int minLength)
    {
        if (string.IsNullOrEmpty(password)) return false;

        for (int length = 1; length <= password.Length / 2; length++)
        {
            for (int i = 0; i <= password.Length - length * 2; i++)
            {
                var pattern = password.Substring(i, length);
                var nextOccurrence = password.IndexOf(pattern, i + length);
                if (nextOccurrence != -1 && nextOccurrence < i + length * 2)
                {
                    return true; // Found a repeated pattern
                }
            }
        }
        return false;
    }

}
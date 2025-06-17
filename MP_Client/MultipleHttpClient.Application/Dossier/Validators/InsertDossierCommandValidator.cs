using FluentValidation;
using MultipleHttpClient.Application.Dossier.Command;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class InsertDossierCommandValidator : AbstractValidator<InsertDossierCommand>
    {
        public InsertDossierCommandValidator()
        {
            // Required GUID fields
            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("Status ID is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            // Optional GUID fields
            When(x => x.DemandTypeId.HasValue, () => {
                RuleFor(x => x.DemandTypeId!.Value)
                    .NotEmpty().WithMessage("Demand Type ID cannot be empty if provided");
            });

            // Similar rules for other optional GUID fields
            When(x => x.ActivityNatureId.HasValue, () => {
                RuleFor(x => x.ActivityNatureId!.Value)
                    .NotEmpty().WithMessage("Activity Nature ID cannot be empty if provided");
            });

            // String fields validation
            RuleFor(x => x.LocalAddress)
                .MaximumLength(200).WithMessage("Local address must be less than 200 characters")
                .When(x => !string.IsNullOrEmpty(x.LocalAddress));

            RuleFor(x => x.Area)
                .MaximumLength(50).WithMessage("Area must be less than 50 characters")
                .When(x => !string.IsNullOrEmpty(x.Area));

            // Price validation
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be positive")
                .When(x => x.Price.HasValue);

            // Coordinate validation
            RuleFor(x => x.Latitude)
                .Matches(@"^-?([1-8]?[0-9]\.\d+|90\.0+)$")
                .When(x => !string.IsNullOrEmpty(x.Latitude))
                .WithMessage("Invalid latitude format");

            RuleFor(x => x.Longitude)
                .Matches(@"^-?((1[0-7]|[0-9])?[0-9]\.\d+|180\.0+)$")
                .When(x => !string.IsNullOrEmpty(x.Longitude))
                .WithMessage("Invalid longitude format");

            // Conditional coordinate validation
            RuleFor(x => x.Longitude)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Latitude))
                .WithMessage("Longitude is required when Latitude is provided");

            RuleFor(x => x.Latitude)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Longitude))
                .WithMessage("Latitude is required when Longitude is provided");

            // ICE validation
            RuleFor(x => x.ICE)
                .Length(15).When(x => !string.IsNullOrEmpty(x.ICE))
                .WithMessage("ICE must be 15 characters");

            // Base64 image validation
            RuleForEach(x => x.InteriorPhotos)
                .Must(BeValidBase64).When(x => x.InteriorPhotos != null)
                .WithMessage("Invalid Base64 format for interior photo");

            RuleForEach(x => x.ExteriorPhotos)
                .Must(BeValidBase64).When(x => x.ExteriorPhotos != null)
                .WithMessage("Invalid Base64 format for exterior photo");

            // Business rule validation
            RuleFor(x => x.OpeningHours)
                .MaximumLength(100).WithMessage("Opening hours must be less than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.OpeningHours));

            RuleFor(x => x.OpeningDays)
                .MaximumLength(100).WithMessage("Opening days must be less than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.OpeningDays));

            RuleFor(x => x.YearsOfExperience)
                .MaximumLength(50).WithMessage("Years of experience must be less than 50 characters")
                .When(x => !string.IsNullOrEmpty(x.YearsOfExperience));

            RuleFor(x => x.LocalComment)
                .MaximumLength(500).WithMessage("Local comment must be less than 500 characters")
                .When(x => !string.IsNullOrEmpty(x.LocalComment));

            // Facade validation
            RuleFor(x => x.Facade)
                .MaximumLength(100).WithMessage("Facade must be less than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Facade));

            // Potential validation
            RuleFor(x => x.Potential)
                .InclusiveBetween(0, 100).When(x => x.Potential.HasValue)
                .WithMessage("Potential must be between 0 and 100");

            // Fiscal identification validation 
            RuleFor(x => x.FiscalIdentification)
                .MaximumLength(100).WithMessage("Fiscal identification must be less than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.FiscalIdentification));
        }

        private bool BeValidBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;
            try
            {
                // Check if the string is properly formatted Base64
                var buffer = new Span<byte>(new byte[base64String.Length]);
                return Convert.TryFromBase64String(base64String, buffer, out _);
            }
            catch
            {
                return false;
            }
        }
    }
}

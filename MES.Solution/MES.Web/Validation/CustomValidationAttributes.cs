using System.ComponentModel.DataAnnotations;

namespace MES.Web.Validation
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext context)
        {
            if (value is Guid guid && guid == Guid.Empty)
                return new ValidationResult(
                    ErrorMessage ?? "Please select a valid option.");

            return ValidationResult.Success;
        }
    }

    public class DateGreaterThanAttribute(string comparisonProperty) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext context)
        {
            if (value is not DateTime endDate)
                return ValidationResult.Success;

            var property = context.ObjectType.GetProperty(comparisonProperty);
            if (property is null)
                return new ValidationResult($"Property {comparisonProperty} not found.");

            var startDate = (DateTime?)property.GetValue(context.ObjectInstance);

            if (startDate.HasValue && endDate <= startDate.Value)
                return new ValidationResult(
                    ErrorMessage ?? "End date must be after start date.");

            return ValidationResult.Success;
        }
    }
}

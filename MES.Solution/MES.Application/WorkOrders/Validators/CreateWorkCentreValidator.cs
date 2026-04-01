

using FluentValidation;
using MES.Application.DTOs;

namespace MES.Application.WorkOrders.Validators
{
    public class CreateWorkCentreValidator : AbstractValidator<CreateWorkCentreDto>
    {
        public CreateWorkCentreValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(20).WithMessage("Code cannot exceed 20 characters.")
                .Matches("^[A-Za-z0-9-]+$")
                .WithMessage("Code can only contain letters, numbers, and hyphens.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");

            RuleFor(x => x.CapacityPerShift)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero.");
        }
    }
}

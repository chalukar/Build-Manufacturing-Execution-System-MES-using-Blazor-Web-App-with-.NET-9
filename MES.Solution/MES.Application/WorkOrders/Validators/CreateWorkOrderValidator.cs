using FluentValidation;
using MES.Application.DTOs;

namespace MES.Application.WorkOrders.Validators
{
    public class CreateWorkOrderValidator : AbstractValidator<CreateWorkOrderDto>
    {
        public CreateWorkOrderValidator() {
            RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("Product code is required.")
            .MaximumLength(50);

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200);

            RuleFor(x => x.PlannedQuantity)
                .GreaterThan(0).WithMessage("Planned quantity must be greater than zero.");

            RuleFor(x => x.PlannedStartDate)
                .LessThan(x => x.PlannedEndDate)
                .WithMessage("Start date must be before end date.");

            RuleFor(x => x.WorkCentreId)
                .NotEmpty().WithMessage("Work centre is required.");
        }
    }
}

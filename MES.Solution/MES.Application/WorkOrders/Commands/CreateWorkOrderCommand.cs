using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Entities;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;


namespace MES.Application.WorkOrders.Commands
{
    public record CreateWorkOrderCommand(CreateWorkOrderDto Dto) : IRequest<Result<Guid>>;

    public class CreateWorkOrderCommandHandler(IUnitOfWork uow) : IRequestHandler<CreateWorkOrderCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var workCentre = await uow.WorkCentres.GetByIdAsync(dto.WorkCentreId, cancellationToken)
                ?? throw new NotFoundException(nameof(WorkCentre), dto.WorkCentreId);

            var number = $"WO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

            if (await uow.WorkOrders.ExistsAsync(number, cancellationToken))
                return Result<Guid>.Failure("Work order number already exists.");

            var workOrder = WorkOrder.Create(
                number,
                dto.ProductCode,
                dto.ProductName,
                dto.PlannedQuantity,
                dto.PlannedStartDate,
                dto.PlannedEndDate,
                dto.WorkCentreId,
                dto.Notes);

            await uow.WorkOrders.AddAsync(workOrder, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(workOrder.Id);
        }
    }
}

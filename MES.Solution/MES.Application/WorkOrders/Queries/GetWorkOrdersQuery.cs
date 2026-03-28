using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Enums;
using MES.Domain.Interfaces;

namespace MES.Application.WorkOrders.Queries
{
    public record GetWorkOrdersQuery(WorkOrderStatus? Status = null) : IRequest<Result<IReadOnlyList<WorkOrderDto>>>;
    public class GetWorkOrdersQueryHandler(IUnitOfWork uow) : IRequestHandler<GetWorkOrdersQuery, Result<IReadOnlyList<WorkOrderDto>>>
    {
        public async Task<Result<IReadOnlyList<WorkOrderDto>>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = request.Status.HasValue
            ? await uow.WorkOrders.GetByStatusAsync(request.Status.Value, cancellationToken)
            : await uow.WorkOrders.GetAllAsync(cancellationToken);

            var dtos = orders.Select(wo => new WorkOrderDto(
                wo.Id, wo.WorkOrderNumber, wo.ProductCode, wo.ProductName,
                wo.PlannedQuantity, wo.ProducedQuantity, wo.ScrappedQuantity,
                wo.Status, wo.PlannedStartDate, wo.PlannedEndDate,
                wo.ActualStartDate, wo.ActualEndDate,
                wo.WorkCentre?.Name ?? string.Empty,
                wo.GetEfficiency(), wo.Notes))
                .ToList()
                .AsReadOnly();

            return Result<IReadOnlyList<WorkOrderDto>>.Success(dtos);
        }
    }
}

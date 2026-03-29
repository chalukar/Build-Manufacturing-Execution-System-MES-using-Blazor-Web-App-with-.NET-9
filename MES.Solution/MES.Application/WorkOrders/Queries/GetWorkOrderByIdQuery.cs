using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Entities;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.WorkOrders.Queries
{
    public record GetWorkOrderByIdQuery(Guid Id) : IRequest<Result<WorkOrderDto>>;
    public class GetWorkOrderByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrderDto>>
    {
        public async Task<Result<WorkOrderDto>> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var wo = await uow.WorkOrders
                .GetWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(WorkOrder), request.Id);

            var dto = new WorkOrderDto(
                wo.Id,
                wo.WorkOrderNumber,
                wo.ProductCode,
                wo.ProductName,
                wo.PlannedQuantity,
                wo.ProducedQuantity,
                wo.ScrappedQuantity,
                wo.Status,
                wo.PlannedStartDate,
                wo.PlannedEndDate,
                wo.ActualStartDate,
                wo.ActualEndDate,
                wo.WorkCentre?.Name ?? string.Empty,
                wo.GetEfficiency(),
                wo.Notes);

            return Result<WorkOrderDto>.Success(dto);

        }
    }
}

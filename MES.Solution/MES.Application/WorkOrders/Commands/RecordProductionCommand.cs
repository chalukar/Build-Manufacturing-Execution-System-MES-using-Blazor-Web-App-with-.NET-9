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

namespace MES.Application.WorkOrders.Commands
{
    public record RecordProductionCommand(RecordProductionDto Dto, string OperatorId) : IRequest<Result<bool>>;
    public class RecordProductionCommandHandler(IUnitOfWork uow) : IRequestHandler<RecordProductionCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(RecordProductionCommand request, CancellationToken cancellationToken)
        {
            var wo = await uow.WorkOrders.GetByIdAsync(request.Dto.WorkOrderId, cancellationToken)
            ?? throw new NotFoundException("WorkOrder", request.Dto.WorkOrderId);

            wo.RecordProduction(request.Dto.GoodQuantity, request.Dto.ScrapQuantity);

            var entry = ProductionEntry.Create(
                wo.Id,
                request.Dto.GoodQuantity,
                request.Dto.ScrapQuantity,
                request.OperatorId,
                request.Dto.Remarks);

            uow.WorkOrders.Update(wo);
            await uow.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    
    }
}

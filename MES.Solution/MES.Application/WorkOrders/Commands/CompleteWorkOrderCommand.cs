using MediatR;
using MES.Application.Common;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.WorkOrders.Commands
{
    public record CompleteWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<bool>>;
    public class CompleteWorkOrderCommandHandler(IUnitOfWork uow) : IRequestHandler<CompleteWorkOrderCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CompleteWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var wo = await uow.WorkOrders
                .GetByIdAsync(request.WorkOrderId, cancellationToken)
                ?? throw new NotFoundException("WorkOrder", request.WorkOrderId);

                wo.Complete();
                uow.WorkOrders.Update(wo);
                await uow.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}

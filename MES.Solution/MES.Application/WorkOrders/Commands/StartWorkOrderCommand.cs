using MediatR;
using MES.Application.Common;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;


namespace MES.Application.WorkOrders.Commands
{
    public record StartWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<bool>>;
    public class StartWorkOrderCommandHandler(IUnitOfWork uow) : IRequestHandler<StartWorkOrderCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(StartWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var wo = await uow.WorkOrders
                .GetByIdAsync(request.WorkOrderId, cancellationToken)
                ?? throw new NotFoundException("WorkOrder", request.WorkOrderId);

                wo.Start();
                uow.WorkOrders.Update(wo);
                await uow.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}

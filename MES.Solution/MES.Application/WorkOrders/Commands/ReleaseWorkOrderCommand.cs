using MediatR;
using MES.Application.Common;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;


namespace MES.Application.WorkOrders.Commands
{
    public record ReleaseWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<bool>>;
    public class ReleaseWorkOrderCommandHandler(IUnitOfWork uow) : IRequestHandler<ReleaseWorkOrderCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ReleaseWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var wo = await uow.WorkOrders.GetByIdAsync(request.WorkOrderId, cancellationToken)
                ?? throw new NotFoundException("WorkOrder", request.WorkOrderId);

                wo.Release();
                uow.WorkOrders.Update(wo);
                await uow.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true);
        }
    }
}

using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Entities;
using MES.Domain.Interfaces;

namespace MES.Application.WorkOrders.Commands
{
    public record CreateWorkCentreCommand(CreateWorkCentreDto CreateWorkCentreDto) : IRequest<Result<Guid>>;

    public class CreateWorkCentreCommandHandler(IUnitOfWork uow) : IRequestHandler<CreateWorkCentreCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateWorkCentreCommand request, CancellationToken cancellationToken)
        {
            var cwcdto = request.CreateWorkCentreDto;

            var existing = await uow.WorkCentres.GetByCodeAsync(cwcdto.Code.Trim().ToUpper(), cancellationToken);

            if (existing is not null)
                return Result<Guid>.Failure($"Work centre with code '{cwcdto.Code.ToUpper()}' already exists.");

            var workCentre = WorkCentre.Create(
                cwcdto.Code,
                cwcdto.Name,
                cwcdto.Department,
                cwcdto.CapacityPerShift);

            await uow.WorkCentres.AddAsync(workCentre, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(workCentre.Id);
        }
    }
}

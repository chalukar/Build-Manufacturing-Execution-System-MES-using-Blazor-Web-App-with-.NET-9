

using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Entities;
using MES.Domain.Exceptions;
using MES.Domain.Interfaces;

namespace MES.Application.WorkCentres.Queries
{
    public record GetWorkCentreByIdQuery(Guid Id) : IRequest<Result<WorkCentreDto>>;

    public class GetWorkCentreByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetWorkCentreByIdQuery, Result<WorkCentreDto>>
    {
        public async Task<Result<WorkCentreDto>> Handle(GetWorkCentreByIdQuery request, CancellationToken cancellationToken)
        {
            var wc = await uow.WorkCentres
                .GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(WorkCentre), request.Id);

            return Result<WorkCentreDto>.Success(new WorkCentreDto(
                wc.Id,
                wc.Code,
                wc.Name,
                wc.Department,
                wc.CapacityPerShift,
                wc.IsActive));
        }
    }
}

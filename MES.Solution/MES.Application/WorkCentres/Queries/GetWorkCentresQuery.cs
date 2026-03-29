using MediatR;
using MES.Application.Common;
using MES.Application.DTOs;
using MES.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.WorkCentres.Queries
{
    public record GetWorkCentresQuery : IRequest<Result<IReadOnlyList<WorkCentreDto>>>;

    public class GetWorkCentresQueryHandler(IUnitOfWork uow)
        : IRequestHandler<GetWorkCentresQuery, Result<IReadOnlyList<WorkCentreDto>>>
    {
        public async Task<Result<IReadOnlyList<WorkCentreDto>>> Handle(GetWorkCentresQuery request, CancellationToken cancellationToken)
        {
            var workCentres = await uow.WorkCentres
                .GetAllAsync(cancellationToken);

            var dtos = workCentres
                .Where(wc => wc.IsActive)
                .Select(wc => new WorkCentreDto(
                    wc.Id,
                    wc.Code,
                    wc.Name,
                    wc.Department,
                    wc.CapacityPerShift,
                    wc.IsActive))
                .ToList()
                .AsReadOnly();

            return Result<IReadOnlyList<WorkCentreDto>>.Success(dtos);
        }
    }
}

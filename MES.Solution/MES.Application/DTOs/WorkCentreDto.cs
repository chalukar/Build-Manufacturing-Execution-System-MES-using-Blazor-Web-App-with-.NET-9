using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.DTOs
{
    public record WorkCentreDto(
        Guid Id,
        string Code,
        string Name,
        string Department,
        decimal CapacityPerShift,
        bool IsActive);

    public record CreateWorkCentreDto(
        string Code,
        string Name,
        string Department,
        decimal CapacityPerShift);

}

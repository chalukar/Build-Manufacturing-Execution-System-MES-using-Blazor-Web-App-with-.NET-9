using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.DTOs
{
    public record CreateWorkOrderDto(
        string ProductCode,
        string ProductName,
        decimal PlannedQuantity,
        DateTime PlannedStartDate,
        DateTime PlannedEndDate,
        Guid WorkCentreId,
        string? Notes);
}

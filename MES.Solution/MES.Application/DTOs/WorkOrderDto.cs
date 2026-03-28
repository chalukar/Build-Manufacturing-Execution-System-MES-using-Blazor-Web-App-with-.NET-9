using MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.DTOs
{
    public record WorkOrderDto(
        Guid Id,
        string WorkOrderNumber,
        string ProductCode,
        string ProductName,
        decimal PlannedQuantity,
        decimal ProducedQuantity,
        decimal ScrappedQuantity,
        WorkOrderStatus Status,
        DateTime PlannedStartDate,
        DateTime PlannedEndDate,
        DateTime? ActualStartDate,
        DateTime? ActualEndDate,
        string WorkCentreName,
        decimal EfficiencyPercent,
        string? Notes);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Application.DTOs
{
    public record RecordProductionDto(
        Guid WorkOrderId,
        decimal GoodQuantity,
        decimal ScrapQuantity,
        string? Remarks);
}

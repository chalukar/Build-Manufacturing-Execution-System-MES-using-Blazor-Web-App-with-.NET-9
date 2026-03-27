using MES.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Entities
{
    public class ProductionEntry : BaseEntity
    {
        public Guid WorkOrderId { get; private set; }
        public decimal GoodQuantity { get; private set; }
        public decimal ScrapQuantity { get; private set; }
        public string OperatorId { get; private set; } = string.Empty;
        public DateTime RecordedAt { get; private set; }
        public string? Remarks { get; private set; }

        public WorkOrder WorkOrder { get; private set; } = null!;

        private ProductionEntry() { }

        public static ProductionEntry Create(Guid workOrderId, decimal good, decimal scrap, string operatorId, string? remarks = null)
            => new()
            {
                WorkOrderId = workOrderId,
                GoodQuantity = good,
                ScrapQuantity = scrap,
                OperatorId = operatorId,
                RecordedAt = DateTime.UtcNow,
                Remarks = remarks
            };
    }
}

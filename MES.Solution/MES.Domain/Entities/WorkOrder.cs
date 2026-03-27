using MES.Domain.Common;
using MES.Domain.Enums;
using MES.Domain.Exceptions;

namespace MES.Domain.Entities
{
    public class WorkOrder : BaseEntity
    {
        public string WorkOrderNumber { get; private set; } = string.Empty;
        public string ProductCode { get; private set; } = string.Empty;
        public string ProductName { get; private set; } = string.Empty;
        public decimal PlannedQuantity { get; private set; }
        public decimal ProducedQuantity { get; private set; }
        public decimal ScrappedQuantity { get; private set; }
        public WorkOrderStatus Status { get; private set; }
        public DateTime PlannedStartDate { get; private set; }
        public DateTime PlannedEndDate { get; private set; }
        public DateTime? ActualStartDate { get; private set; }
        public DateTime? ActualEndDate { get; private set; }
        public Guid WorkCentreId { get; private set; }
        public string? Notes { get; private set; }

        // Navigation
        public WorkCentre WorkCentre { get; private set; } = null!;
        public IReadOnlyCollection<ProductionEntry> ProductionEntries => _productionEntries.AsReadOnly();
        public IReadOnlyCollection<QualityInspection> QualityInspections => _qualityInspections.AsReadOnly();

        private readonly List<ProductionEntry> _productionEntries = [];
        private readonly List<QualityInspection> _qualityInspections = [];


        // EF Core constructor
        private WorkOrder() { }

       public static WorkOrder Create(
           string workOrderNumber,
           string productCode,
           string productName,
           decimal plannedQuantity,
           DateTime plannedStartDate,
           DateTime plannedEndDate,
           Guid workCentreId,
           string? notes = null)
        {
            if (plannedQuantity <= 0)
                throw new DomainException("Planned quantity must be greater than zero.");

            if (plannedEndDate <= plannedStartDate)
                throw new DomainException("End date must be after start date.");

            return new WorkOrder
            {
                WorkOrderNumber = workOrderNumber,
                ProductCode = productCode,
                ProductName = productName,
                PlannedQuantity = plannedQuantity,
                PlannedStartDate = plannedStartDate,
                PlannedEndDate = plannedEndDate,
                WorkCentreId = workCentreId,
                Notes = notes,
                Status = WorkOrderStatus.Draft
            };
        }
   

        public void Release()
        {
            if (Status != WorkOrderStatus.Draft)
                throw new DomainException($"Cannot release work order in status {Status}.");
            Status = WorkOrderStatus.Released;
        }

        public void Start()
        {
            if (Status != WorkOrderStatus.Released)
                throw new DomainException($"Cannot start work order in status {Status}.");
            Status = WorkOrderStatus.InProgress;
            ActualStartDate = DateTime.UtcNow;
        }

        public void RecordProduction(decimal quantity, decimal scrapped)
        {
            if (Status != WorkOrderStatus.InProgress)
                throw new DomainException("Can only record production on in-progress work orders.");

            if (quantity < 0 || scrapped < 0)
                throw new DomainException("Quantities cannot be negative.");

            ProducedQuantity += quantity;
            ScrappedQuantity += scrapped;
        }

        public void Complete()
        {
            if (Status != WorkOrderStatus.InProgress)
                throw new DomainException($"Cannot complete work order in status {Status}.");
            Status = WorkOrderStatus.Completed;
            ActualEndDate = DateTime.UtcNow;
        }

        public decimal GetEfficiency() =>
            PlannedQuantity > 0 ? ProducedQuantity / PlannedQuantity * 100 : 0;
    }
}

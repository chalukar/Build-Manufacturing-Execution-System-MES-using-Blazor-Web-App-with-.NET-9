using MES.Domain.Common;

namespace MES.Domain.Entities
{
    public class WorkCentre : BaseEntity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Department { get; private set; } = string.Empty;
        public decimal CapacityPerShift { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<WorkOrder> _workOrders = [];
        public IReadOnlyCollection<WorkOrder> WorkOrders => _workOrders.AsReadOnly();

        private WorkCentre() { }

        public static WorkCentre Create(string code, string name, string department, decimal capacity)
            => new() 
            { 
                Code = code, 
                Name = name, 
                Department = department, 
                CapacityPerShift = capacity, 
                IsActive = true 
            };

        public void Update(string name, string department, decimal capacity)
        {
            Name = name.Trim();
            Department = department.Trim();
            CapacityPerShift = capacity;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}

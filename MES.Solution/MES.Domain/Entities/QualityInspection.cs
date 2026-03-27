using MES.Domain.Common;
using MES.Domain.Enums;


namespace MES.Domain.Entities
{
    public class QualityInspection : BaseEntity
    {
        public Guid WorkOrderId { get; private set; }
        public string InspectorId { get; private set; } = string.Empty;
        public QualityInspectionResult Result { get; private set; }
        public decimal SampleSize { get; private set; }
        public decimal DefectsFound { get; private set; }
        public string? Comments { get; private set; }
        public DateTime InspectedAt { get; private set; }

        public WorkOrder WorkOrder { get; private set; } = null!;

        private QualityInspection() { }

        public static QualityInspection Create(Guid workOrderId, string inspectorId, decimal sampleSize,
        decimal defects, string? comments = null)
        {
            var result = defects == 0
                ? QualityInspectionResult.Passed
                : defects / sampleSize < 0.05m
                    ? QualityInspectionResult.ConditionalPass
                    : QualityInspectionResult.Failed;

            return new QualityInspection
            {
                WorkOrderId = workOrderId,
                InspectorId = inspectorId,
                SampleSize = sampleSize,
                DefectsFound = defects,
                Result = result,
                Comments = comments,
                InspectedAt = DateTime.UtcNow
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Enums
{
    public enum WorkOrderStatus
    {
        Draft = 0,
        Released = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4,
        OnHold = 5
    }
}

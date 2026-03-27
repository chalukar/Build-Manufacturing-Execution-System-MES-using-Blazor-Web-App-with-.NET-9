using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Enums
{
    public enum QualityInspectionResult
    {
        Pending = 0,
        Passed = 1,
        Failed = 2,
        ConditionalPass = 3
    }
}

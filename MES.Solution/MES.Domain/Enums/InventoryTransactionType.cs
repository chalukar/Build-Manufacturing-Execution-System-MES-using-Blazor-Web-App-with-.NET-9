using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Enums
{
    public enum InventoryTransactionType
    {
        GoodsReceipt = 0,
        GoodsIssue = 1,
        StockTransfer = 2,
        Adjustment = 3
    }
}

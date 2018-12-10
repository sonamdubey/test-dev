using System;

namespace Carwale.Entity.Stock
{
    [Serializable]
    public class StockPackageInfo
    {
        public int PackageId { get; set; }
        public DateTime? PackageStartDate { get; set; }
    }
}

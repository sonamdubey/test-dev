using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class DealerStatusEntity
    {
        public bool IsDealerMissing { get; set; }
        public bool IsPackageMissing { get; set; }
        public DateTime? PackageStartDate { get; set; }
        public DateTime? PackageEndDate { get; set; }
        public bool IsMigrated { get; set; }
    }
}

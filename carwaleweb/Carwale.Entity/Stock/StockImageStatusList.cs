using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageStatusList
    {
        public int? SellerType { get; set; }
        public List<StockImageStatus> Status { get; set; }
    }
}

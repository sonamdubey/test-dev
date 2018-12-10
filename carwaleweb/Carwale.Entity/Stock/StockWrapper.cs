using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
     [Serializable]
    public class StockWrapper
    {
        public Stock Stock { get; set; }        
        public string OperationType { get; set; }
    }
}

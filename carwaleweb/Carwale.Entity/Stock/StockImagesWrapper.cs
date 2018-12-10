using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    [Serializable]
    public class StockImagesWrapper
    {
        public StockImageList StockImages { get; set; }        
        public string OperationType { get; set; }
    }
}

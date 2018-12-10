using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageAcknowledgement
    {
        public int ImageId { get; set; }
        public int StockId { get; set; }
        public string Action { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
    }
}

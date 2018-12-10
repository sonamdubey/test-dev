using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockAcknowledgement
    {
        public int StockId { get; set; }
        public int DealerId { get; set; }
        public string Action { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public string ProfileId { get; set; }
        public string Url { get; set; }
    }
}

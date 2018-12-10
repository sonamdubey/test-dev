using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class DealsInquiryDetail
    {
        public int RecordId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public int CustomerCity { get; set; }
        public int StockId { get; set; }
        public ulong ResponseId { get; set; }
        public int CityId { get; set; }
        public int MasterCityId { get; set; }
        public string Source { get; set; }
        public short IsPaid { get; set; }
        public string MultipleStockId { get; set; }
        public int PlatformId { get; set; }
        public int Eagerness { get; set; }
        public int ABTestValue { get; set; }
    }
}

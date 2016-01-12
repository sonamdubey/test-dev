using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>    
    /// Created by  :   Sumit Kate on 08 Jan 2016
    /// Summary     :   Dealer Price Quote SMS Entity
    /// </summary>
    public class DPQSmsEntity
    {
        public string LandingPageShortUrl { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string DealerName { get; set; }
        public string Locality { get; set; }
        public string DealerMobile { get; set; }
        public string BikeName { get; set; }
        public UInt32 BookingAmount { get; set; }
    }
}

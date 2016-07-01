using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerPriceQuoteList
    {
        public IEnumerable<DealerPriceQuoteDetailed> DealersDetails { get; set; }
    }
}
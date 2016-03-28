using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 21 March 2016
    /// Description : DealersList for dealer locator
    /// </summary>
    public class DealersList : Bikewale.Entities.PriceQuote.NewBikeDealerBase
    {
        public AreaEntityBase Area { get; set; }
        public DealerPackageTypes Type { get; set; }
        public string City { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
    }
}

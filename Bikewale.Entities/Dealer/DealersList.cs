using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 21 March 2016
    /// Description : DealersList for dealer locator
    /// </summary>
    public class DealersList : Bikewale.Entities.PriceQuote.NewBikeDealerBase
    {
        public AreaEntityBase objArea { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }
}

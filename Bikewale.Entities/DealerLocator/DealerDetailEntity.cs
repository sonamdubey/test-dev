using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 21 March 2016
    /// Description : for Dealer Detail.
    /// </summary>
    public class DealerDetailEntity : NewBikeDealerBase
    {
        public AreaEntityBase Area { get; set; }
        public DealerPackageTypes Type { get; set; }
        public string City { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public DateTime ShowRoomStartTime { get; set; }
        public DateTime ShowRoomEndTime { get; set; }
        public string WorkingHours { get; set; }
    }
}

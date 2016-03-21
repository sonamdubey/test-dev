using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : for Dealer Details. 
    /// </summary>
    public class DealerDetail : DealerBase
    {
        public PQAreaBase Area { get; set; }
        public DealerPackageType Type { get; set; }
        public string City { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public DateTime ShowRoomStartTime { get; set; }
        public DateTime ShowRoomEndTime { get; set; }
        public string WorkingHours { get; set; }
    }
}

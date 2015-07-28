using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Location;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by : Ashwini Todkar on 4 June 2014
    /// </summary>
    public class NewBikeDealersListEntity
    {
        public IList<CityWiseDealersCountEntity> CityWiseDealers { get; set; }
        public IList<StateEntityBase> StatesList { get; set; }
        public int TotalDealers { get; set; }
    }
}

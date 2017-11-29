using Bikewale.Entities.Location;
using System.Collections.Generic;

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

using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Mar 2017
    /// Description :   Most Popular Bike Widget VM
    /// </summary>
    public class MostPopularBikeWidgetVM
    {
        public uint PageCatId { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public IEnumerable<MostPopularBikesBase> Bikes { get; set; }
    }
}

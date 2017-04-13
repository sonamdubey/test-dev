using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by Sajal Gupta on 7-4-2017
    /// Description : UpcomingBikeResult entity
    /// </summary>
    public class UpcomingBikeResult
    {
        public IEnumerable<UpcomingBikeEntity> Bikes { get; set; }
        public UpcomingBikesListInputEntity Filter { get; set; }
        public uint TotalCount { get; set; }
    }
}

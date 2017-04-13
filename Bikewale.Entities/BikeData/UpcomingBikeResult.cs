using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    public class UpcomingBikeResult
    {
        public IEnumerable<UpcomingBikeEntity> Bikes { get; set; }
        public UpcomingBikesListInputEntity Filter { get; set; }
        public uint TotalCount { get; set; }
    }
}

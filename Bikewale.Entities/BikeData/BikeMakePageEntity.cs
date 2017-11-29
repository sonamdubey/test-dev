using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    public class BikeMakePageEntity
    {
        public BikeDescriptionEntity Description { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularBikes { get; set; }
    }
}

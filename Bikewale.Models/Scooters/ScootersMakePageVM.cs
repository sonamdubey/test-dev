using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class ScootersMakePageVM
    {
        public BikeMakeEntityBase Make { get; set; }

        public IEnumerable<MostPopularBikesBase> Bikes { get; set; }
        public IEnumerable<Bikewale.Entities.BikeData.SimilarCompareBikeEntity> SimilarComparisons { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
    }
}

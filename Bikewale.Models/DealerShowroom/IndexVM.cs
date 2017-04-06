
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models
{
    public class IndexVM : ModelBase
    {
        public NewLaunchedWidgetVM NewLaunchedBikes { get; set; }
        public UpcomingBikesWidgetVM objUpcomingBikes { get; set; }
        public BestBikeWidgetVM BestBikes { get; set; }
        public IEnumerable<BikeMakeEntityBase> MakesList { get; set; }
        public BrandWidgetVM Brands { get; set; }
    }
}

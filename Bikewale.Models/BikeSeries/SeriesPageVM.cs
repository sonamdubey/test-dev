using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Models.BikeSeries
{
    public class SeriesPageVM : ModelBase
    {
        public BikeDescriptionEntity SeriesDescription { get; set; }
        public BikeMakeBase BikeMake { get; set; }
        public BikeSeriesEntityBase SeriesBase { get; set; }
        public BikeSeriesModels SeriesModels { get; set; }
        public CityEntityBase City { get; set; }
		public OtherSeriesVM OtherSeries { get; set; }
        public RecentNewsVM News { get; set; }
        public BikeSeriesCompareVM ObjModel { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public UsedBikeByModelCityVM objUsedBikes { get; set; }
        public PopularComparisonsVM TopComparisons { get; set; }

    }
}

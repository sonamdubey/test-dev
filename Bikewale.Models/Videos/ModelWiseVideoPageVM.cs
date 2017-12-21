using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Videos;
using Bikewale.Models.BikeSeries;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 11 Dec 2017
    /// Description : Added PopularSeriesBikes
    /// </summary>
    public class ModelWiseVideoPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
        public BikeInfoVM BikeInfoWidgetData { get; set; }
        public IEnumerable<SimilarBikeWithVideo> SimilarBikeVideoList { get; set; }
        public uint CityId { get; set; }
        public BikeSeriesEntityBase objSeries { get; set; }
        public EnumBikeBodyStyles bikeType {get; set;}
        public PopularSeriesBikesVM PopularSeriesBikes { get; set; }
    }
}

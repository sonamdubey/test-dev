using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Videos;
using Bikewale.Models.BikeSeries;
using System.Collections.Generic;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 11 Dec 2017
    /// Modified by: Snehal Dange on 21th dec 2017
    /// Summary : added MoreAboutScootersWidgetVM
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
        public EnumBikeBodyStyles bikeType { get; set; }
        public PopularSeriesBikesVM PopularSeriesBikes { get; set; }
        public MoreAboutScootersWidgetVM ObjMoreAboutScooter { get; set; }
    }
}

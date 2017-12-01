using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Videos;

namespace Bikewale.Models.Videos
{
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
    }
}

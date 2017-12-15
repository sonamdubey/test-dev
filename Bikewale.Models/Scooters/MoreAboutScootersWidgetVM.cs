
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class MoreAboutScootersWidgetVM
    {
        public BikeInfoVM BikeInfo { get; set; }
        public IEnumerable<SimilarBikeEntity> SimilarBikes { get; set; }
        public IEnumerable<BestBikeEntityBase> objBestBikesList { get; set; }
        public BikeModelEntity objBikeData { get; set; }

    }
}

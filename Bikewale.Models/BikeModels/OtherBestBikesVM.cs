using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Models.BikeModels
{
    public class OtherBestBikesVM
    {
        public bool IsMakePresentInConfig { get; set; }
        public string OtherBestBikesHeading { get; set; }
        public IEnumerable<BestBikeEntityBase> BestBikes { get; set; }
    }
}

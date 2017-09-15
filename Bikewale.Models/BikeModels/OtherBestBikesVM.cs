using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 15 Sep 2017
    /// Summary     :   View model for similar bikes and more bikes from make widget
    /// </summary>
    public class OtherBestBikesVM
    {
        public bool IsMakePresentInConfig { get; set; }
        public string OtherBestBikesHeading { get; set; }
        public IEnumerable<BestBikeEntityBase> BestBikes { get; set; }
    }
}

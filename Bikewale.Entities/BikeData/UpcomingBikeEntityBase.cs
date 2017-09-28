using Bikewale.Entities.Images;
using System;

namespace Bikewale.Entities.BikeData
{
    public class UpcomingBikeEntityBase
    {
        public BikeMakeBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public DateTime ExpectedLaunchDate { get; set; }
        public PriceEntityBase ExpectedPrice { get; set; }
        public ImageEntityBase BikeImage { get; set; }
        public uint Count { get; set; }
    }
}

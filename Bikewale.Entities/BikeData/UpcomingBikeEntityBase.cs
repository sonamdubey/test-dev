using Bikewale.Entities.Images;

namespace Bikewale.Entities.BikeData
{
    public class UpcomingBikeEntityBase
    {
        public string ExpectedLaunch { get; set; }
        public uint Count { get; set; }
        public BikeMakeBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public ImageEntityBase BikeImage { get; set; }
        public PriceEntityBase ExpectedPrice { get; set; }
        
    }
}

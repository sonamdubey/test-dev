using Bikewale.Entities.Images;

namespace Bikewale.Entities.BikeData
{
    public class NewBikeEntityBase
    {
        public BikeMakeEntityBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public PriceEntityBase ExpectedPrice { get; set; }
        public Image BikeImage { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public uint Count { get; set; }
    }
}

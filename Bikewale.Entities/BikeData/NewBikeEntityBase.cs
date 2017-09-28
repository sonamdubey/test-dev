using Bikewale.Entities.Images;

namespace Bikewale.Entities.BikeData
{

    public class NewBikeEntityBase
    {
        public BikeMakeBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public ImageEntityBase BikeImage { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public PriceEntityBase Price { get; set; }
        public uint Count { get; set; }
    }
}

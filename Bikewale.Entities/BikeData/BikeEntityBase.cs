
namespace Bikewale.Entities.BikeData
{
    public class BasicBikeEntityBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
    }
}

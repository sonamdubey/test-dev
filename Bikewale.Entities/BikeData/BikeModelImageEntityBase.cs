
namespace Bikewale.Entities.BikeData
{
    [System.Serializable]
    public class BikeModelImageEntityBase : Images.ImageEntityBase
    {
        public BikeMakeBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
    }
}

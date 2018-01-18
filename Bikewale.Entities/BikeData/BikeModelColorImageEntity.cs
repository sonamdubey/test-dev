
namespace Bikewale.Entities.BikeData
{
    [System.Serializable]
    public class BikeModelColorImageEntity : BikeModelImageEntityBase
    {
        public uint ColorId { get; set; }
        public uint PhotoId { get; set; }
        public ushort PhotosCount { get; set; }
    }
}

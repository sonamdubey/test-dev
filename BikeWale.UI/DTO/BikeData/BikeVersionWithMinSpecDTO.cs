
namespace Bikewale.DTO.BikeData
{
    public class BikeVersionWithMinSpecDTO
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        public long OnRoadPrice { get; set; }

        public string BrakingSystem { get; set; }
        public string BrakeType { get; set; }
        public string WheelType { get; set; }
        public string StartType { get; set; }
    }
}

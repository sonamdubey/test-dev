
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Dec 2017
    /// Entity to hold dealer pricing for model versions
    /// </summary>
    public class BikeVersionWithMinSpec
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

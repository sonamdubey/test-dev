
namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Created By vivek gupta on 24 june 2016
    /// Desc : to add lat long dealer count
    /// </summary>
    public class DealerCityEntity : CityEntityBase
    {
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public uint DealersCount { get; set; }
    }
}

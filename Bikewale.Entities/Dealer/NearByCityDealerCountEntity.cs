using Bikewale.Entities.Location;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by : Sajal Gupta on 19-12-2016
    /// Desc : Entity for dealer count in nearby city
    /// </summary>
    public class NearByCityDealerCountEntity : CityEntityBase
    {
        public uint DealersCount { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
    }
}

using Bikewale.Entities.Location;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : Entity to hold price and city entity
    /// </summary>
    public class PriceAndCityEntityBase
    {
        public PriceEntityBase Price { get; set; }
        public CityEntityBase City { get; set; }
    }
}

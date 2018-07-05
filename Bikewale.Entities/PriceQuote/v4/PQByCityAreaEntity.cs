using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote.v4
{
    /// <summary>
    /// Created By : Sushil Kumar on 1st August 2016
    /// Summary   : Entity for pricequote when city,area and modelId are passed 
    ///             to generate pricequote and tale actions accordingly
    /// </summary>
    public class PQByCityAreaEntity
    {
        public Bikewale.Entities.BikeBooking.v2.PQOutputEntity PriceQuote { get; set; }
        public IEnumerable<CityEntityBase> PQCitites { get; set; }
        public IEnumerable<Bikewale.Entities.Location.AreaEntityBase> PQAreas { get; set; }

    }
}

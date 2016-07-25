using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerPriceEntity
    {
        public PQ_Price Price { get; set; }
        public BikeVersionEntityBase Version { get; set; }
        public CityEntityBase City { get; set; }
    }
}

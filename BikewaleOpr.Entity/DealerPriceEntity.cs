using BikewaleOpr.Entities;

namespace BikeWaleOpr.Entities
{

    public class DealerPriceEntity
    {
        public PQ_Price Price { get; set; }
        public VersionEntityBase Version { get; set; }
        public CityEntityBase City { get; set; }
    }
}

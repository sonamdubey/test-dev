using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikePricing
{
    public class PriceMonitoringEntity
    {
        public IEnumerable<Entities.MfgCityEntity> CityList { get; set; }
        public IEnumerable<PriceLastUpdateEntity> PriceLastUpdatedList{ get; set; }
        public IEnumerable<BikeVersionEntityBase> BikeVersionList { get; set; }
        public IEnumerable<BikeModelEntityBase> BikeModelList { get; set; }
    }
}

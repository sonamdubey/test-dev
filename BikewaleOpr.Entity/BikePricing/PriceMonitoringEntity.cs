using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikePricing
{
    public class PriceMonitoringEntity
    {
        public IEnumerable<MfgCityEntity> CityList { get; set; }
        public IEnumerable<PriceLastUpdateEntity> PriceLastUpdatedList{ get; set; }
        public IEnumerable<BikeVersionEntity> BikeVersionList { get; set; }
    }
}

using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Entities.DealerLocator
{
    public class StateCityEntity
    {
        public string Link { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string stateMaskingName { get; set; }
        public uint DealerCountState { get; set; }
        public uint totalDealerIndia { get; set; }
        public uint totalCities { get; set; }
        public IEnumerable<DealerCityEntity> Cities { get; set; }
    }
}

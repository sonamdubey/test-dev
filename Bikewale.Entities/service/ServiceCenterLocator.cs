
using System.Collections.Generic;
namespace Bikewale.Entities.service
{
    public class ServiceCenterLocator
    {
        public string Link { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string stateMaskingName { get; set; }
        public uint ServiceCenterCountState { get; set; }
        public uint totalDealerIndia { get; set; }
        public uint totalCities { get; set; }
        public IEnumerable<ServiceCityEntity> Cities { get; set; }


    }
}

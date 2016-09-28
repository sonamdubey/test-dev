using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Entities.DealerLocator
{
    public class DealerListIndia
    {
        public ushort cityid { get; set; }
        public string cityName { get; set; }
        public string cityMaskingName { get; set; }
        public string cityLattitude { get; set; }
        public string cityLongitude { get; set; }
        public ushort stateId { get; set; }
        public string stateName { get; set; }
        public string stateMaskingName { get; set; }
        public string stateLattitude { get; set; }
        public string stateLongitude { get; set; }
        public int dealerCountCity { get; set; }

    }

    public class StateCityEntity
    {
        public string Link { get; set; }
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string stateMaskingName { get; set; }
        public uint DealerCountState { get; set; }
        public List<DealerCityEntity> Cities { get; set; }

    }
}

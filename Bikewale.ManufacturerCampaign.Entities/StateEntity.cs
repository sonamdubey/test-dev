using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class StateEntity
    {
        public uint StateId { get; set; }
        public string StateName { get; set; }
        public IEnumerable<CityEntity> Cities { get; set; }
    }
}

using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 22 Jun 2017
    /// Summary    : Entity for states
    /// </summary>
    public class StateEntity
    {
        public uint StateId { get; set; }
        public string StateName { get; set; }
        public IEnumerable<CityEntity> Cities { get; set; }
    }
}

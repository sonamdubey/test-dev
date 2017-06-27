using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class ManufacturerCampaignRulesEntity
    {
        public BikeMakeEntity Make { get; set; }
        public BikeModelEntity Model { get; set; }
        public IEnumerable<StateEntity> State { get; set; }
       
    }
}

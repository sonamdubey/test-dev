using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Jun 2017
    /// Summary    : Wrapper entity for all rules of a bike model
    /// </summary>
    public class ManufacturerCampaignRulesEntity
    {
        public BikeMakeEntity Make { get; set; }
        public BikeModelEntity Model { get; set; }
        public IEnumerable<StateEntity> State { get; set; }
       
    }
}

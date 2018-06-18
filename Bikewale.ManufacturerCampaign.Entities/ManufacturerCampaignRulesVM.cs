using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 22 June 2017
    /// Summary    : View Model for manufacturer campaign rules
    /// Modified by : Rajan Chauhan on 4 May 2018
    /// Description : Added field UnmappedModels (To be used for keeping list of unmapped honda models for Honda Campaign)
    /// </summary>
    public class ManufacturerCampaignRulesVM
    {
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public IEnumerable<ManufacturerCampaignRulesEntity> Rules { get; set; }
        public IEnumerable<BikeMakeEntity> Makes { get; set; }
        public IEnumerable<BikeModelEntity> UnmappedModels { get; set; }
        public IEnumerable<StateEntity> States { get; set; }
        public NavigationWidgetEntity NavigationWidget { get; set; }
        public bool ShowOnExShowroom { get; set; }
    }
}

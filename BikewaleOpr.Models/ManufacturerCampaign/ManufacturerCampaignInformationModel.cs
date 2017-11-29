using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities.ContractCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ManufacturerCampaignInformationModel
    {
        public ConfigureCampaignEntity CampaignInformation {get; set;}
        public uint CampaignId { get; set; }
        public IEnumerable<MaskingNumber> MaskingNumbers { get; set; }     
        public NavigationWidgetEntity NavigationWidget { get; set; }
    }
}

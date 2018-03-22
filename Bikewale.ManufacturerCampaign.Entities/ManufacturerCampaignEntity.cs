using System;
namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign Entity
    /// </summary>
    [Serializable]
    public class ManufacturerCampaignEntity
    {
        public ManufacturerCampaignLeadConfiguration LeadCampaign { get; set; }
        public ManufacturerCampaignEMIConfiguration EMICampaign { get; set; }
    }
}


using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.Entities.manufacturecampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacture Campaign LeadEntity
    /// </summary>
    public class ManufactureCampaignLeadEntity : ManufacturerCampaignLeadConfiguration
    {
        public int LeadSourceId { get; set; }
        public int PqSourceId { get; set; }
        public string MakeName { get; set; }
        public string Area { get; set; }
        public string GAAction { get; set; }
        public string GACategory { get; set; }
        public string GALabel { get; set; }
        public uint PQId { get; set; }
        public uint VersionId { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacture Campaign EMI Entity
    /// </summary>
    public class ManufactureCampaignEMIEntity : ManufacturerCampaignEMIConfiguration
    {
        public int LeadSourceId { get; set; }
        public int PqSourceId { get; set; }
        public string MakeName { get; set; }
        public string Area { get; set; }
        public string GAAction { get; set; }
        public string GACategory { get; set; }
        public string GALabel { get; set; }
    }
}

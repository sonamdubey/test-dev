
namespace Bikewale.Entities.manufacturecampaign
{
    /// <summary>
    /// Created by subodh Jain 29 Aug 2016
    /// Description For manufacture Campaign 
    /// </summary>
    class ManufactureCampaignInput
    {
        public int CampaignId { get; set; }

        public string ManufacturerName { get; set; }

        public string MaskingNumber { get; set; }

        public int DealerId { get; set; }

        public string DealerArea { get; set; }

        public int LeadsourceId { get; set; }

        public int LeadpqsourceId { get; set; }

        public string GAAction { get; set; }

        public string GACategory { get; set; }

        public string GALabel { get; set; }

        public bool IsMaskingNumner { get; set; }
    }
}

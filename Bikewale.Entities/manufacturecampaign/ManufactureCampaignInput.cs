
namespace Bikewale.Entities.manufacturecampaign
{
    class ManufactureCampaignInput
    {
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

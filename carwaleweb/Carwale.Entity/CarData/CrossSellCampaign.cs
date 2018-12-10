using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CrossSellCampaign
    {
        public int ModelId { get; set; } 
        public int VersionId { get; set; } 
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public int DealerLeadBusinessType { get; set; } 
        public string DealerName { get; set; } 
        public int CampaignId { get; set; } 
        public string HostURL { get; set; } 
        public string MaskingName { get; set; }
        public string OriginalImgPath { get; set; }
        public long OnRoadPrice { get; set; } 
        public string MaskingNumber { get; set; } 
        public bool ShowEmail { get; set; } 
        public int LeadPanel { get; set; }
        public int MakeId { get; set; }
        public List<LeadSource> LeadSource { get; set; }
    }
}

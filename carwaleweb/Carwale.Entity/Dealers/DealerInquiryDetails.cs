using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Customers;
using Newtonsoft.Json;
using Carwale.Entity.Campaigns;

namespace Carwale.Entity.Dealers
{
    [JsonObject]
    [Serializable]
    public class DealerInquiryDetails : CustomerMinimal
    {
        public int VersionId { get; set; }
        public int DealerId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ZoneId { get; set; }
        public int AreaId { get; set; }
        public string BuyTimeText { get; set; }
        public int BuyTimeValue { get; set; }
        public int RequestType { get; set; }
        public string InquirySourceId { get; set; } // for Autobiz
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public ulong PQId { get; set; }
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public int ApplicationId { get; set; }
        public int LeadBussinessType { get; set; }
        public int LeadClickSource { get; set; }
        public int PlatformSourceId { get; set; }
        public ulong PQDealerAdLeadId { get; set; }
        public string EncryptedPQDealerAdLeadId { get; set; }
        public int AssignedDealerId { get; set; } // Chosen by the Customer
        public bool IsAutoApproved { get; set; }
        public int CampaignId { get; set; } // added by ashish Verma
        public string Ltsrc { get; set; }
        public int ActualDealerId { get; set; } // Owner of the lead
        public string Comments { get; set; }
        public string UtmaCookie { get; set; }
        public string UtmzCookie { get; set; }
        public string Platform { get; set; }
        public string UserAgent { get; set; }
        public string LeadSourceName { get; set; }
        public string LeadSourceCategoryId { get; set; }
        public string LeadSourceId { get; set; }
        public bool NewLead { get; set; }
        public int DealsStockId { get; set; }
        public bool IsPaymentSuccess { get; set; }
        public string ModelsHistory { get; set; }
        public bool IsPushToThirdParty { get; set; }
        public UInt16 ABTest { get; set; }
        public string CwCookie { get; set; }
        public int LeadPushSource { get; set; }
        public string SponsoredBannerCookie { get; set; }
        public bool? LeadDuplication { get; set; }
        public int LandingPageId { get; set; }
        public string ClientIP { get; set; }
        public PredictionModelRequest predictionModelRequest { get; set; }
        public ulong OriginalLeadId { get; set; }
        public Dictionary<string,string> Others { get; set; }
        public short PageId { get; set; }
        public short PropertyId { get; set; }
        public short SourceType { get; set; }
        public bool IsCitySet { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

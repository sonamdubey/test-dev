using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTO.Dealers;
using Newtonsoft.Json;
using Carwale.DTOs.Dealer;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("contactName")]
        public string ContactName { get; set; }
        [JsonProperty("contactNumber")]
        public string ContactNumber { get; set; }
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }
        [JsonProperty("type")]
        public short Type { get; set; }
        [JsonProperty("actionText")]
        public string ActionText { get; set; }
        [JsonProperty("isEmailRequired")]
        public bool IsEmailRequired { get; set; }
        [JsonProperty("leadPanel")]
        public short LeadPanel { get; set; }
        [JsonProperty("showroomDealer")]
        public DealersDTO DealerShowroom { get; set; }
        [JsonProperty("cvlDetails")]
        public CvlDetailsDTO CvlDetails { get; set; }
        [JsonProperty("predictionData")]
        public PredictionData PredictionData { get; set; }
        [JsonProperty("mutualLeads")]
        public bool MutualLeads { get; set; }
        [JsonProperty("dealerAdminId")]
        public int DealerAdminId { get; set; }
        [JsonProperty("isTestDriveCampaign")]
        public bool IsTestDriveCampaign { get; set; }
        [JsonProperty("isTurboMla")]
        public bool IsTurboMla { get; set; }
        [JsonProperty("maskingNumberEnabled")]
        public bool MaskingNumberEnabled { get; set; }

        public CampaignDTO()
        {
            this.CvlDetails = new CvlDetailsDTO();
        }

    }

    public class CvlDetailsDTO
    {
        [JsonProperty("isCvl")]
        public bool IsCvl { get; set; }
    }
}

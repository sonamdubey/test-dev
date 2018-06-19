using Newtonsoft.Json;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 21th October 2015
    /// Modified By :   Sumit Kate on 18 Aug 2016
    /// Description :   Removed the private variable and kept only public properties
    /// Modifier    : Kartik rathod on 15 may 2018 , added dealername and bikename,SendLeadSMSCustomer
    /// </summary>
    public class ManufacturerLeadEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("pqId")]
        public uint PQId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("leadSourceId")]
        public ushort LeadSourceId { get; set; }

        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("manufacturerDealerId")]
        public uint ManufacturerDealerId { get; set; }

        [JsonProperty("manufacturerDealer")]
        public string ManufacturerDealer { get; set; }

        [JsonProperty("manufacturerDealerCity")]
        public string ManufacturerDealerCity { get; set; }

        [JsonProperty("manufacturerDealerState")]
        public string ManufacturerDealerState { get; set; }

        [JsonProperty("leadId")]
        public uint LeadId { get; set; }

        [JsonProperty("dealerName")]
        public string DealerName { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("sendLeadSMSCustomer")]
        public bool SendLeadSMSCustomer { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 2 Feb 2017
    /// Description :   Gaadi.com Lead Entity
    /// </summary>
    public class GaadiLeadEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("sub_source")]
        public string Source { get; set; }
    }
}

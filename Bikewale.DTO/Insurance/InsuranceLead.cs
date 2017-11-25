using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore on 19 November 2015 
    /// Description : For Detail of the user who submitted Insurance Form.
    /// </summary>
    class InsuranceLead
    {
        [JsonProperty("customerId")]
        public ulong CustomerId{ get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        
        [JsonProperty("cityName")]
        public string CityName{ get; set; }

        [JsonProperty("stateName")]
        public string StateName{ get; set; }

        [JsonProperty("cityId")]
        public int CityId{ get; set; }

        [JsonProperty("email")]
        public string Email{ get; set; }

        [JsonProperty("mobile")]
        public string Mobile{ get; set; }

        [JsonProperty("clientIP")]
        public string ClientIP{ get; set; }

        [JsonProperty("insurancePolicyType")]
        public int InsurancePolicyType{ get; set; }

        [JsonProperty("policyExpiryDate")]
        public DateTime PolicyExpiryDate{ get; set; }

        [JsonProperty("bikeRegistrationDate")]
        public DateTime BikeRegistrationDate{ get; set; }

        [JsonProperty("makeName")]
        public string MakeName{ get; set; }

        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName{ get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName{ get; set; }

        [JsonProperty("versionId")]
        public int VersionId{ get; set; }

        [JsonProperty("makeYear")]
        public string MakeYear{ get; set; }

        [JsonProperty("clientPrice")]
        public int ClientPrice{ get; set; }

        [JsonProperty("noClaimBonus")]
        public int NoClaimBonus{ get; set; }

        [JsonProperty("requestDateTime")]
        public DateTime RequestDateTime{ get; set; }

        [JsonProperty("submitStatus")]
        public string SubmitStatus{ get; set; }

        [JsonProperty("submitStatusId")]
        public int SubmitStatusId{ get; set; } //

        [JsonProperty("clientId")]
        public int ClientId{ get; set; }
        //e.g. PolicyBoss

        [JsonProperty("leadSourceId")]
        public int LeadSourceId{ get; set; } 
    }
}

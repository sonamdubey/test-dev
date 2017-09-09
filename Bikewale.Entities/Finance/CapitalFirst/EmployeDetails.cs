using Newtonsoft.Json;


namespace Bikewale.Entities.Finance.CapitalFirst
{
   public class EmployeDetails
    {

        [JsonProperty("status")]
        public ushort Status { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("officalAddressLine1")]
        public string OfficalAddressLine1 { get; set; }

        [JsonProperty("officalAddressLine2")]
        public string OfficalAddressLine2 { get; set; }

        [JsonProperty("pincode")]
        public uint Pincode { get; set; }

        [JsonProperty("annualIncome")]
        public uint AnnualIncome { get; set; }
    }
}

using Bikewale.Entities.Dealer;
using Newtonsoft.Json;

namespace Bikewale.Entities.Finance.CapitalFirst
{
    public class PersonalDetails
    {
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }

        [JsonProperty("ctLeadId")]
        public uint CtLeadId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("pincode")]
        public string Pincode { get; set; }

        [JsonProperty("pancard")]
        public string Pancard { get; set; }


        [JsonProperty("status")]
        public ushort Status { get; set; }
        /// <summary>
        /// capital first table id
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }

        public string objLeadJson { get; set; }
        public ManufacturerLeadEntity objLead { get; set; }
    }
}

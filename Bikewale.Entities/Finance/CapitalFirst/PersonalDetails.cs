using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.Finance.CapitalFirst
{
    public class PersonalDetails
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("mobileNumber")]
        public uint MobileNumber { get; set; }
        [JsonProperty("emailId")]
        public string EmailId { get; set; }
        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
  
        [JsonProperty("gender")]
        public ushort Gender { get; set; }

        [JsonProperty("maritalStatus")]
        public ushort MaritalStatus { get; set; }

        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("pincode")]
        public uint Pincode { get; set; }

        [JsonProperty("pancard")]
        public string Pancard { get; set; }


    }
}

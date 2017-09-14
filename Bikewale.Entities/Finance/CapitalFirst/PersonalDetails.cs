﻿using Bikewale.Entities.Dealer;
using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.Finance.CapitalFirst
{
    public class PersonalDetails
    {
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }

        [JsonProperty("ctLeadId")]
        public uint CTLeadId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

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
        public string Pincode { get; set; }

        [JsonProperty("pancard")]
        public string Pancard { get; set; }


        [JsonProperty("status")]
        public ushort Status { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("officialAddressLine1")]
        public string OfficialAddressLine1 { get; set; }

        [JsonProperty("officialAddressLine2")]
        public string OfficialAddressLine2 { get; set; }

        [JsonProperty("pincodeOffice")]
        public string PincodeOffice { get; set; }

        [JsonProperty("annualIncome")]
        public uint AnnualIncome { get; set; }

        /// <summary>
        /// capital first table id
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }

        public string objLeadJson { get; set; }
        public ManufacturerLeadEntity objLead { get; set; }
    }
}

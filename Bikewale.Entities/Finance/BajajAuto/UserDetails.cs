using Bikewale.Entities.Dealer;
using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.Finance.BajajAuto
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 20 Apr 2018.
    /// Description : Entity for Bajaj Auto Finance user details.
    /// </summary>
    public class UserDetails
    {
        [JsonProperty("bajajAutoId")]
        public uint BajajAutoId { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }
        [JsonProperty("salutation")]
        public byte Salutation { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }
        [JsonProperty("emailId")]
        public string EmailId { get; set; }
        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("landMark")]
        public string LandMark { get; set; }
        [JsonProperty("pinCode")]
        public string PinCode { get; set; }
        [JsonProperty("residenceStatus")]
        public string ResidenceStatus { get; set; }
        [JsonProperty("residingSince")]
        public string ResidingSince { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("employmentType")]
        public byte EmploymentType { get; set; }
        [JsonProperty("workingSince")]
        public byte WorkingSince { get; set; }
        [JsonProperty("companyId")]
        public uint CompanyId { get; set; }
        [JsonProperty("otherCompany")]
        public string OtherCompany { get; set; }
        [JsonProperty("primaryIncome")]
        public string PrimaryIncome { get; set; }
        [JsonProperty("dependents")]
        public byte Dependents { get; set; }
        [JsonProperty("bajajSupplierId")]
        public uint BajajSupplierId { get; set; }
        [JsonProperty("likelyPurchaseDate")]
        public DateTime LikelyPurchaseDate { get; set; }
        [JsonProperty("repaymentMode")]
        public string RepaymentMode { get; set; }
        [JsonProperty("idProof")]
        public string IdProof { get; set; }
        [JsonProperty("idProofNo")]
        public string IdProofNo { get; set; }
        [JsonProperty("bankAccountNo")]
        public string BankAccountNo { get; set; }
        [JsonProperty("accountVintage")]
        public string AccountVintage { get; set; }

        [JsonProperty("pinCodeId")]
        public uint PinCodeId { get; set; }
        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }
        public string LeadJson { get; set; }
        [JsonIgnore]
        public string ResponseJson { get; set; }
        [JsonIgnore]
        public ManufacturerLeadEntity ManufacturerLead { get; set; }
        [JsonIgnore]
        public bool IsMobileVerified { get; set; }
        [JsonIgnore]
        public UInt64 RefEnqNumber { get; set; }
    }

     [Serializable]
    public class BajajBikeMappingEntity
    {
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint StateId { get; set; }
        public uint CityId { get; set; }
        public string PinCode { get; set; }
    }
}

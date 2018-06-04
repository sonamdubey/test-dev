using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.Finance.CapitalFirst
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 Sep 2017
    /// Description :   Capital First Voucher Entity Base
    /// </summary>
    public class CapitalFirstVoucherEntityBase
    {
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string AgentName { get; set; }
        public string AgentContactNumber { get; set; }
        public CapitalFirstVoucherStatus Status { get; set; }
    }


    public enum CapitalFirstVoucherStatus
    {
        Pre_Approved = 3,
        Rejected = 4,
        Credit_Refer = 5
    }

    public class CTFormResponse
    {
        [JsonProperty("status")]
        public ushort Status { get; set; }
        [JsonProperty("details")]
        public String Details { get; set; }
        [JsonProperty("message")]
        public String Message { get; set; }
        [JsonProperty("lead_id")]
        public uint LeadId { get; set; }
        [JsonProperty("lead_status")]
        public ushort LeadStatus { get; set; }
        [JsonProperty("sales_officer")]
        public string SalesOfficer { get; set; }
        [JsonProperty("sales_officer_mobile")]
        public string SalaesOfficerMobile { get; set; }
    }
}

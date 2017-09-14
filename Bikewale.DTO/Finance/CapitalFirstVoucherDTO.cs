using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Finance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 Sep 2017
    /// Desription  :   Capital First Voucher DTO
    /// </summary>
    public class CapitalFirstVoucherDTO
    {
        [JsonProperty("voucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("voucherExpiryDate")]
        public DateTime ExpiryDate { get; set; }
        [JsonProperty("agentName")]
        public string AgentName { get; set; }
        [JsonProperty("agentContactNumber")]
        public string AgentContactNumber { get; set; }
        [JsonProperty("status")]
        public CapitalFirstVoucherStatusDTO Status { get; set; }
    }

    public enum CapitalFirstVoucherStatusDTO
    {
        Pre_Approved = 3,
        Rejected = 4,
        Credit_Refer = 5
    }
}

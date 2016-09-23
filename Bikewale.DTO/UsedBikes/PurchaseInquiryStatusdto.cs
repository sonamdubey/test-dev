using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Status DTO
    /// </summary>
    public class PurchaseInquiryStatusDTO
    {
        [JsonProperty("status")]
        public UInt16 Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}

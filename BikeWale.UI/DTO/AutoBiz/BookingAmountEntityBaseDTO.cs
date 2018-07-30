using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 15 dec 2014
    /// </summary>
    /// 
    public class BookingAmountEntityBaseDTO
    {
        [JsonProperty("id")]
        public UInt32 Id { get; set; }

        [JsonProperty("amount")]
        public UInt32 Amount { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}

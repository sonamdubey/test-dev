using Bikewale.DTO.PriceQuote.BikeBooking;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v4
{
    public class PQCustomerDetailOutput
    {
        [JsonProperty("pqId")]
        public UInt64 PQId { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("dealer")]
        public DealerDetailsDTO Dealer { get; set; }

        [JsonProperty("noOfAttempts")]
        public sbyte NoOfAttempts { get; set; }

        [JsonProperty("leadId")]
        public uint LeadId { get; set; }

        [JsonProperty("guId")]
        public string GuId { get; set; }
    }
}
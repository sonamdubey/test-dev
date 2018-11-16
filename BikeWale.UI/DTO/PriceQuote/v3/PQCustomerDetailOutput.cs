﻿using Bikewale.DTO.PriceQuote.BikeBooking;
using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created by  : Pratibha Verma on 26 June 2018
    /// Description : new version for Bikewale.DTO.PriceQuote.PQCustomerDetailOutput(added leadId)
    /// </summary>
    public class PQCustomerDetailOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("dealer")]
        public DealerDetailsDTO Dealer { get; set; }

        [JsonProperty("noOfAttempts")]
        public sbyte NoOfAttempts { get; set; }

        [JsonProperty("leadId")]
        public uint LeadId { get; set; }

    }
}

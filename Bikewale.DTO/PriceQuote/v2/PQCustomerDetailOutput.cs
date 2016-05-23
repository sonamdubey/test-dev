using Bikewale.DTO.PriceQuote.BikeBooking;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Price quote customer details output DTO version 2
    /// Author  :   Sumit Kate
    /// Date    :   23 May 2016
    /// </summary>
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
    }
}

﻿using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New PQPrimaryDealer version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class PQOutput
    {
        [JsonProperty("quoteId")]
        public ulong PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("isDealerAvailable")]
        public bool IsDealerAvailable { get; set; }
    }
}

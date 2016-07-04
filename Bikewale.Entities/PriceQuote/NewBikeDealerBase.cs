﻿using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified on : 15 March 2016
    /// Description : for Dealer Basics details.
    /// Modified By : Lucky Rathore
    /// Modified on : 21 March 2016
    /// Description : DealerPkgType added.
    /// </summary>
    public class NewBikeDealerBase
    {
        [JsonProperty("id")]
        public UInt32 DealerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("dealerPackageType")]
        public DealerPackageTypes DealerPackageType { get; set; }
    }
}

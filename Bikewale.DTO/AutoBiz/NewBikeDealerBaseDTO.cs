using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Author      :   Sumit Kate on 14 Mar 2016
    /// Description :   NewBikeDealerBase Entity
    /// Modified By :   Sumit Kate on 21 Mar 2016
    /// Description :   Dealer package Id
    /// </summary>
    public class NewBikeDealerBaseDTO
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
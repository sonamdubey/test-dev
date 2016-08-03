using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 17th June 2016
    /// Description : Dealer Basics details with version prices.
    /// Modified by :   Sumit Kate on 03 Aug 2016
    /// Description :   Added new property SelectedVersionPrice
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

        [JsonProperty("isPremium")]
        public bool IsPremiumDealer { get { return (DealerPackageType == DealerPackageTypes.Premium ? true : false); } }

        [JsonProperty("versions")]
        public IEnumerable<VersionPriceEntity> Versions { get; set; }

        [JsonProperty("offerCount")]
        public UInt16 OfferCount { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("selectedVersionPrice")]
        public uint SelectedVersionPrice { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 17th June 2016
    /// Description : Dealer Basics details with version prices.
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

    }
}

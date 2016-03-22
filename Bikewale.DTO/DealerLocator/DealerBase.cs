using Bikewale.DTO.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 22 March 2016
    /// Summary : DTO for Dealer Base for dealer Locator.
    /// </summary>
    public class DealerBase
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
        public DealerPackageType DealerPkgType { get; set; }
    }
}

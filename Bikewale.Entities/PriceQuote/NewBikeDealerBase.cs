using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : for Dealer Basics details.
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

        [JsonProperty("dealerPkgType")]
        public DealerPackageTypes DealerPkgType { get; set; }
    }
}

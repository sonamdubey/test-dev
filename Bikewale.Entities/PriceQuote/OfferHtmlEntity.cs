using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    public class OfferHtmlEntity
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Dealer
{
    public class NewBikeDealerList
    {
        [JsonProperty("dealers")]
        public IEnumerable<NewBikeDealerBase> Dealers { get; set; }

        [JsonProperty("totalDealers")]
        public int TotalDealers { get; set; }
    }
    public class OfferHtmlEntity
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

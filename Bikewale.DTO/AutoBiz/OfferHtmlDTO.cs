using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// Entity Class for Get terms for Offer
    /// </summary>
    public class OfferHtmlEntityDTO
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

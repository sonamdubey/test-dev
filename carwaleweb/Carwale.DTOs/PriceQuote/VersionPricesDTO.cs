using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    /// <summary>
    /// Created By : Satish on 3 Nov 2015
    /// </summary>
    public class VersionPricesDTO
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("onRoadPrice")]
        public int OnRoadPrice { get; set; }
        [JsonProperty("basePrice")]
        public int BasePrice { get; set; }
        [JsonProperty("prices")]
        public IEnumerable<PriceDTO> Prices { get; set; }
    }
}

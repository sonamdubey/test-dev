using Carwale.Entity;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    [Serializable]
    public class PQOnRoadPrice
    {
        public List<PQItem> PriceQuoteList { get; set; }
        public long OnRoadPrice { get; set; }
    }

    public class PQOnRoadPriceDTO
    {
        private List<PQItemDTO> _priceQuoteList = new List<PQItemDTO>();
        [JsonProperty("priceQuoteList")]
        public List<Carwale.DTOs.PriceQuote.PQItemDTO> PriceQuoteList { get { return _priceQuoteList; } set { _priceQuoteList = value; } }
        [JsonProperty("onRoadPrice")]
        public long OnRoadPrice { get; set; }
    }

    public class Prices
    {
        [JsonProperty("metallic")]
        public PQOnRoadPriceDTO Metalic { get; set; }
        [JsonProperty("solid")]
        public PQOnRoadPriceDTO Solid { get; set; }
    }
}

using Bikewale.DTO.BikeData;
using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// On Road Price Quote DTO
    /// Author  :   Sumit Kate
    /// Created On  :   08 Sept 2015
    /// </summary>
    public class PQOnRoad
    {
        [JsonProperty("priceQuote")]
        public PQOutput PriceQuote { get; set; }
        [JsonProperty("dealerPriceQuote")]
        public DPQuotationOutput DPQOutput { get; set; }
        [JsonProperty("bwPriceQuote")]
        public PQBikePriceQuoteOutput BPQOutput { get; set; }
        public bool IsDealerPriceAvailable { get { if (this.DPQOutput != null) { return true; } else { return false; } } }
        [JsonProperty("isInsuranceFree")]
        public bool IsInsuranceFree { get; set; }
        [JsonProperty("insuranceAmount")]
        public uint InsuranceAmount { get; set; } 
        [JsonProperty("bikeDetails")]
        public ModelDetail BikeDetails { get; set; }
    }
}

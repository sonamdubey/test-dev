using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer Price Quote price category
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class DPQ_Price
    {
        [JsonProperty("categoryId")]
        public UInt32 CategoryId { get; set; }
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("price")]
        public UInt32 Price { get; set; }
        [JsonProperty("dealerId")]
        public UInt32 DealerId { get; set; }
    }
}

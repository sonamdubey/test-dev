using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed Dealer Quotation Price category entity
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQPQ_Price
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

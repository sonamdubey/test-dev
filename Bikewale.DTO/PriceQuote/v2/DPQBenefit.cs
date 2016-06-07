
using Newtonsoft.Json;
namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQBenefit version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class DPQBenefit
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

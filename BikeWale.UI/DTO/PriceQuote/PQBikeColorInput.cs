using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Bike color input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQBikeColorInput
    {
        [JsonProperty("pqId")]
        public uint PQId { get; set; }
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
    }
}

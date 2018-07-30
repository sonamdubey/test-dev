using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Bike Color output
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQBikeColorOutput
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }
    }
}

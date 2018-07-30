using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Created     :   16 Oct 2015
    /// Description :   PQ Update output DTO
    /// </summary>
    public class PQUpdateOutput
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }
    }
}

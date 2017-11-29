using Newtonsoft.Json;

namespace Bikewale.DTO.BikeBooking
{
    public class PagingUrl
    {
        [JsonProperty("prevPageUrl")]
        public string PrevPageUrl { get; set; }
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }
}

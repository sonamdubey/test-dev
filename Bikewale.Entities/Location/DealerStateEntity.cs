
using Newtonsoft.Json;
namespace Bikewale.Entities.Location
{
    public class DealerStateEntity : StateEntityBase
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
        [JsonProperty("dealerCount")]
        public int StateDealerCount { get; set; }
    }
}

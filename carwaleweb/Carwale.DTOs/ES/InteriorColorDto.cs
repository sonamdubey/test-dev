using Carwale.DTOs.CarData;
using Newtonsoft.Json;

namespace Carwale.DTOs.ES
{    
    public class InteriorColorDto : CarColorDTO
    {
        [JsonProperty("carCount")]
        public int CarCount { get; set; }
    }
}

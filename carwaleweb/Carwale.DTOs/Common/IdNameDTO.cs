using Newtonsoft.Json;

namespace Carwale.DTOs.Common
{
    public class IdNameDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Customer
{
    public class CustomerBaseDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }
}

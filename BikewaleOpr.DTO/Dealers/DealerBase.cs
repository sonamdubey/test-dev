using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created by : Aditi Srivastava on 9 Feb 2017
    /// Summary    : Dealers of a make in a city
    /// </summary>
    public class DealerBase
    {
        [JsonProperty("dealerId")]
        public uint Id { get; set; }

        [JsonProperty("dealerName")]
        public string Name { get; set; }

    }
}

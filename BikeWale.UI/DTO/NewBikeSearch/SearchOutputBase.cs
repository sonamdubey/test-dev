using Bikewale.DTO.Model;
using Newtonsoft.Json;

namespace Bikewale.DTO.NewBikeSearch
{
    public class SearchOutputBase
    {
        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("displacement")]
        public float Displacement { get; set; }

        [JsonProperty("fuelType")]
        public string FuelType { get; set; }

        [JsonProperty("power")]
        public string Power { get; set; }

        [JsonProperty("fuelEfficiency")]
        public ushort FuelEfficiency { get; set; }

        [JsonProperty("weight")]
        public ushort KerbWeight { get; set; }

        [JsonProperty("torque")]
        public float MaximumTorque { get; set; }

        [JsonProperty("price")]
        public string FinalPrice { get; set; }

        [JsonProperty("availSpecs")]
        public string AvailableSpecs { get; set; }

        [JsonProperty("bikemodel")]
        public ModelDetail BikeModel { get; set; }
    }
}

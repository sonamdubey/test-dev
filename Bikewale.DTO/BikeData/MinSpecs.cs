using Newtonsoft.Json;
namespace Bikewale.DTO.BikeData
{
    public class MinSpecs
    {
        public float Displacement { get; set; }
        public ushort FuelEfficiencyOverall { get; set; }
        public float MaxPower { get; set; }
        public float MaximumTorque { get; set; }
        [JsonProperty("weight")]
        public float KerbWeight { get; set; }
    }
}

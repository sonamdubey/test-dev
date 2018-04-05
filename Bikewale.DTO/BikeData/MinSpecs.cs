using Newtonsoft.Json;
namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Modified by : Rajan Chauhan on 5 Apr 2017
    /// Description : Changed FuelEfficiencyOverall to float
    /// </summary>
    public class MinSpecs
    {
        public float Displacement { get; set; }
        public float FuelEfficiencyOverall { get; set; }
        public float MaxPower { get; set; }
        public float MaximumTorque { get; set; }
        [JsonProperty("weight")]
        public float KerbWeight { get; set; }
    }
}

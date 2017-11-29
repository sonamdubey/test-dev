using Newtonsoft.Json;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Feature DTO
    /// </summary>
    public class BikeFeatureDTO
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("speedometer")]
        public string Speedometer { get; set; }
        [JsonProperty("tachometer")]
        public bool? Tachometer { get; set; }
        [JsonProperty("tachometerType")]
        public string TachometerType { get; set; }
        [JsonProperty("shiftLight")]
        public bool? ShiftLight { get; set; }
        [JsonProperty("electricStart")]
        public bool? ElectricStart { get; set; }
        [JsonProperty("tripmeter")]
        public bool? Tripmeter { get; set; }
        [JsonProperty("noOfTripmeters")]
        public string NoOfTripmeters { get; set; }
        [JsonProperty("tripmeterType")]
        public string TripmeterType { get; set; }
        [JsonProperty("lowFuelIndicator")]
        public bool? LowFuelIndicator { get; set; }
        [JsonProperty("lowOilIndicator")]
        public bool? LowOilIndicator { get; set; }
        [JsonProperty("lowBatteryIndicator")]
        public bool? LowBatteryIndicator { get; set; }
        [JsonProperty("fuelGauge")]
        public bool? FuelGauge { get; set; }
        [JsonProperty("digitalFuelGauge")]
        public bool? DigitalFuelGauge { get; set; }
        [JsonProperty("pillionSeat")]
        public bool? PillionSeat { get; set; }
        [JsonProperty("pillionFootrest")]
        public bool? PillionFootrest { get; set; }
        [JsonProperty("pillionBackrest")]
        public bool? PillionBackrest { get; set; }
        [JsonProperty("pillionGrabrail")]
        public bool? PillionGrabrail { get; set; }
        [JsonProperty("standAlarm")]
        public bool? StandAlarm { get; set; }
        [JsonProperty("steppedSeat")]
        public bool? SteppedSeat { get; set; }
        [JsonProperty("antilockBrakingSystem")]
        public bool? AntilockBrakingSystem { get; set; }
        [JsonProperty("killswitch")]
        public bool? Killswitch { get; set; }
        [JsonProperty("clock")]
        public bool? Clock { get; set; }   
    }
}

using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Model.v3
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 12 Apr 2016
    /// Description :   This new DTO for Model Page API v3 Version Details
    /// </summary
    public class VersionDetail
    {
        [JsonProperty("brakeType")]
        public string BrakeType { get; set; }

        [JsonProperty("alloyWheels")]
        public bool AlloyWheels { get; set; }

        [JsonProperty("electricStart")]
        public bool ElectricStart { get; set; }

        [JsonProperty("antilockBrakingSystem")]
        public bool AntilockBrakingSystem { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("price")]
        public UInt64 Price { get; set; }

        [JsonProperty("isDealerPriceQuote")]
        public bool IsDealerPriceQuote { get; set; }

    }
}

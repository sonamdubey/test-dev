using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store version information.
    /// Modified by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Added flag for version status
    /// Modified by: Dhruv Joshi
    /// Dated: 21st Feb 2018
    /// Description: Added minspecs as individual properties instead of an entity
    /// Modified by : Kartik rathod on 11 apr 2018 added abs,braketype,starttype,wheels
    /// </summary>
    public class VersionEntity
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }        
        [JsonProperty("priceList")]
        public IEnumerable<PriceEntity> PriceList { get; set; }
        [JsonProperty("versionStatus")]
        public BikeStatus VersionStatus { get; set; }
        [JsonProperty("mileage")]
        public uint Mileage { get; set; }
        [JsonProperty("kerbWeight")]
        public uint KerbWeight { get; set; }
        [JsonProperty("power")]
        public double Power { get; set; }
        [JsonProperty("displacement")]
        public double Displacement { get; set; }
        [JsonProperty("exshowroom")]
        public uint Exshowroom { get; set; }
        [JsonProperty("onroad")]
        public uint Onroad { get; set; }
        [JsonProperty("abs")]
        public bool ABS { get; set; }
        [JsonProperty("brakeType")]
        public string BrakeType { get; set; }
        [JsonProperty("wheels")]
        public string Wheels { get; set; }
        [JsonProperty("startType")]
        public string StartType { get; set; }
    }
 
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
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
    }
}

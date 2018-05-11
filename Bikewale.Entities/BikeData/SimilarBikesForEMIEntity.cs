using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Author  : Kartik Rathod on 11 May 2018
    /// Desc    : similar bikes for emi page in finance
    /// </summary>
    [Serializable]
    public class SimilarBikesForEMIEntity
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("onRoadPrice")]
        public ulong OnRoadPrice { get; set; }
        [JsonProperty("hosturl")]
        public string Hosturl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    public class ModelEntity
    {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
        [JsonProperty("modelStatus")]
        public BikeStatus ModelStatus { get; set; }
    }
}

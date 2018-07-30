using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class ModelList
    {
        [JsonProperty("modelList")] 
        public IEnumerable<ModelBase> Model { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable,JsonObject]
    public class CarMakeModelEntityBase
    {
        
        [JsonProperty(PropertyName = "makeName")]
        public string MakeName { get; set; }

        
        [JsonProperty(PropertyName = "makeId")]
        public string MakeId{ get; set; }

        
        [JsonProperty(PropertyName = "modelName")]
        public string ModelName { get; set; }

        
        [JsonProperty(PropertyName = "modelId")]
        public int ModelId { get; set; }

        
        [JsonProperty(PropertyName = "maskingName")]
        public string MaskingName { get; set; }

        [JsonIgnore]
        public string ImageUrl { get; set; }
    }
}

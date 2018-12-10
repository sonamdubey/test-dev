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
    public class CarModelEntityBase 
    {
        [JsonProperty]
        public int ModelId { get; set; }

        [JsonProperty]
        public string ModelName { get; set; }

        [JsonProperty]
        public string MaskingName { get; set; }

        [JsonProperty]
        public string RootName { get; set; }
    }
}

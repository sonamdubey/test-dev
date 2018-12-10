using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{    
    public class ModelsByRootAndYearDTO
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("modelIds")]        
        public string ModelIds { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }
    }
}

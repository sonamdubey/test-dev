using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model
{
    public class ModelList
    {
        [JsonProperty("modelList")] 
        public IEnumerable<ModelBase> Model { get; set; }
    }
}

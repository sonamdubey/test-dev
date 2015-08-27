using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class ModelList
    {
        [JsonProperty("model")]
        public IEnumerable<ModelBase> Model { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class AdditionalProperty
    {
        [JsonProperty("@type")]
        public string Type { get { return "PropertyValue"; } }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
        
        [JsonProperty("minValue", NullValueHandling = NullValueHandling.Ignore)]
        public string MinValue { get; set; }
        
        [JsonProperty("maxValue", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxValue { get; set; }
        
        [JsonProperty("unitText", NullValueHandling = NullValueHandling.Ignore)]
        public string UnitText { get; set; }

        [JsonProperty("valueReference", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueReference { get; set; }
    }
}

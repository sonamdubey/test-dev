using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class ModelsByRootAndYear
    {
        [DataMember, JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [DataMember, JsonProperty(PropertyName = "modelIds")]
        public string ModelIds { get; set; }

        [DataMember, JsonProperty(PropertyName = "makeName")]
        public string MakeName { get; set; }
    }
}

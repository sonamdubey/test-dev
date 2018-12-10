using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.AutoComplete
{
    [Serializable]
    public class NameValue : LabelValue
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

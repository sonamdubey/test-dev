using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumList
    {
        [JsonProperty("@type")]
        public string Type { get { return "BreadcrumList"; } }

        [JsonProperty("itemListElement")]
        public IEnumerable<BreadcrumListItem> BreadcrumListItem { get; set; }
    }
}

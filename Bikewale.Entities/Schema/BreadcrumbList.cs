using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumbList
    {
        [JsonProperty("@type")]
        public string Type { get { return "BreadcrumbList"; } }

        [JsonProperty("itemListElement")]
        public IEnumerable<BreadcrumbListItem> BreadcrumListItem { get; set; }

    }
}

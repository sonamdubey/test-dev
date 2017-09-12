using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumbListItem : ListItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumbItem Item { get; set; }
    }
}

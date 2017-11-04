using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumbListItem : ListItem
    {
        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumbItem Item { get; set; } 
    }
}

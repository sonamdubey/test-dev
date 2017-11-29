using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To add additional items properties for list items and use it for caraousels
    /// </summary>
    public class ProductListItem : ListItem
    {
        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public Product Item { get; set; }
    }
}

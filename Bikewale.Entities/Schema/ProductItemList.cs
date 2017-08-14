using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class ProductItemList : SchemaBase
    {
        [JsonProperty("@type")]
        public string Type { get { return "ItemList"; } }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("numberOfItems", NullValueHandling = NullValueHandling.Ignore)]
        public uint NumberOfItems { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("itemListOrder", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemListOrder { get; set; }

        [JsonProperty("itemListElement", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Product> ItemListElement { get; set; }
    }

    public class ItemListOrder
    {
        public const string _Ascending = "http://schema.org/ItemListOrderAscending";
        public const string _Descending = "http://schema.org/ItemListOrderDescending";
        public const string _Unordered = "http://schema.org/ItemListUnordered";
    }
}

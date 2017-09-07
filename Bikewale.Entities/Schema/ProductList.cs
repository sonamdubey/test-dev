using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To show list of products (list of bikes)
    /// </summary>
    public class ProductList : SchemaBase
    {
        [JsonProperty("@type")]
        public string Type { get { return "ItemList"; } }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("numberOfItems", NullValueHandling = NullValueHandling.Ignore)]
        public uint NumberOfItems { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("sameAs", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalUrl { get; set; }

        [JsonProperty("itemListOrder", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemListOrder { get; set; }

        [JsonProperty("itemListElement", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ProductListItem> ItemListElement { get; set; }
    }

    public class ItemListOrder
    {
        public const string _Ascending = "http://schema.org/ItemListOrderAscending";
        public const string _Descending = "http://schema.org/ItemListOrderDescending";
        public const string _Unordered = "http://schema.org/ItemListUnordered";
    }
}

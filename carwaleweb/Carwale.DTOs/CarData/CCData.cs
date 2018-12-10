using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class SpecsFeatures
    {
        [JsonProperty("specs", NullValueHandling = NullValueHandling.Ignore)]
        public List<SubCategory> Specs;
        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<SubCategory> Features;
        [JsonProperty("overview", NullValueHandling = NullValueHandling.Ignore)]
        public List<Item> OverView;
    }

    public class SubCategory
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        [JsonProperty("itemMasterId")]
        public string ItemMasterId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }
        [JsonProperty("unitType")]
        public string UnitType { get; set; }
        [JsonIgnore]
        public int DataTypeId { get; set; }
        [JsonIgnore,JsonProperty("description")]
        public string Description { get; set; }
        [JsonIgnore, JsonProperty("tip")]
        public string Tip { get; set; }
        [JsonIgnore,JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("values")]
        public List<string> Values { get; set; }
        [JsonProperty("customTypeId")]
        public int CustomTypeId { get; set; }
    }

}

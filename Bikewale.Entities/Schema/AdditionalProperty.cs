using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To add additional properties to thing or a product type element
    ///                 (it contains key value pair with unit to specify specifications nd other important values)
    /// </summary>
    public class AdditionalProperty
    {
        [JsonProperty("@type")]
        public string Type { get { return "PropertyValue"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("minValue", NullValueHandling = NullValueHandling.Ignore)]
        public string MinValue { get; set; }

        [JsonProperty("maxValue", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxValue { get; set; }

        [JsonProperty("unitText", NullValueHandling = NullValueHandling.Ignore)]
        public string UnitText { get; set; }

        [JsonProperty("valueReference", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueReference { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : Product type to show model details and its properties
    /// </summary>
    public class Product
    {
        [JsonProperty("@type")]
        public string Type { get { return "Product"; } }

        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        [JsonProperty("aggregateRating", NullValueHandling = NullValueHandling.Ignore)]
        public AggregateRating AggregateRating { get; set; }

        [JsonProperty("additionalProperty", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AdditionalProperty> AdditionalProperty { get; set; }

        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Review> Review { get; set; }

        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public Offer Offers { get; set; }

    }
}

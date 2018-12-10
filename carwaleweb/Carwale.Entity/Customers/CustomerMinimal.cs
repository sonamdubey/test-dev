using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Carwale.Entity.Customers
{
    [Serializable, JsonObject]
    public class CustomerMinimal
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string Mobile { get; set; }
    }
}

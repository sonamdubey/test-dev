using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Carwale.Entity.CarData
{
    [Serializable, JsonObject]
    public class RootBase
    {
        [JsonProperty]
        public int RootId { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string MakeName { get; set; }

        [JsonProperty]
        public string MakeId { get; set; }
    }
}

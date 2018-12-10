using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Carwale.Entity.CarData
{
    [Serializable,JsonObject]
    public class CarMakeEntityBase
    {
        
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonIgnore]
        public bool? IsPopular { get; set; }

        [JsonProperty]
        public bool IsDeleted { get; set; }

        [JsonProperty]
        public bool Used { get; set; }

        [JsonProperty]
        public bool New { get; set; }

        [JsonProperty]
        public bool Indian { get; set; }

        [JsonProperty]
        public bool Imported { get; set; }

        [JsonProperty]
        public bool Futuristic { get; set; }

        [JsonProperty]
        public int PriorityOrder { get; set; }
    } 
}

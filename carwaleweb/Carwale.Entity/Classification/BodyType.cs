using Newtonsoft.Json;
using System;

namespace Carwale.Entity.Classification
{
	[Serializable]
    public class BodyType: BodyTypeBase
	{
        [JsonProperty("newCarSearchUrl")]
        public string LandingUrl { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        [JsonIgnore]
        public string LineIcon { get; set; }
    }
}

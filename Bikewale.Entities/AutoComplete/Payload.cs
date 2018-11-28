using Newtonsoft.Json;

namespace Bikewale.Entities.AutoComplete
{
    /// <summary>
    /// Modified By : Monika Korrapati on 22 Nov 2018
    /// Description : Added Label, Type, ExpertReviewsCount
    /// </summary>
    public class Payload
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }

        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("userRatingsCount")]
        public uint UserRatingsCount { get; set; }

        [JsonProperty("expertReviewsCount")]
        public uint ExpertReviewsCount { get; set; }

        [JsonProperty("type")]
        public EntryTypesEnum Type { get; set; }
    }
}

using Newtonsoft.Json;

namespace Bikewale.DTO.AutoComplete.V2
{
    /// <summary>
    /// Created By : Sajal Gupta
    /// Created On : 01/08/2016
    /// Description : Dto for text, makeId and modelId .
    /// </summary>
    public class SuggestionList
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}

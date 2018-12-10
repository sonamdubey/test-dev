using Newtonsoft.Json;
using System;

namespace Carwale.DTOs.Suggestion
{
    public class Base
    {
        [JsonProperty("suggestionType")]
        public Entity.Enum.SuggestionTypeEnum SuggestionType;

        [JsonProperty("superScript")]
        public string SuperScript = string.Empty;

        [JsonProperty("displayName")]
        public string DisplayName;

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo = string.Empty;
        [JsonProperty("payload")]
        public  PayLoad PayLoad;
    }

    public abstract class PayLoad 
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url;
    }

    public class ModelSuggestion : PayLoad
    {       
        [JsonProperty("modelId")]
        public short ModelId;
    }

    public class MakeSuggestion : PayLoad
    {
        
        [JsonProperty("makeId")]
        public short MakeId;
    }

    public class DualCompareSuggestion : PayLoad
    {

        [JsonProperty("modelId1")]
        public short ModelId1;

        [JsonProperty("modelId2")]
        public short ModelId2;

        [JsonProperty("versionId1")]
        public short VersionId1;

        [JsonProperty("versionId2")]
        public short VersionId2;
    }
    public class LinkSuggestion : PayLoad
    {
        [JsonProperty("url")]
        public string Url;
    }
    public class SuggestionInput
    {
        public string source { get; set; }
        public string value { get; set; }
        public int size { get; set; }
        public bool IsAmp { get; set; }
    }
}

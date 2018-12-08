using Newtonsoft.Json;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{
    public class BikewaleInfo
    {
        [JsonProperty("type")]
        public QuestionType Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

}

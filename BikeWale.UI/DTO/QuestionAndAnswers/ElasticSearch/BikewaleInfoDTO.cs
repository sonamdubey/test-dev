using Bikewale.Entities.QuestionAndAnswers.ElasticSearch;
using Newtonsoft.Json;

namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    public class BikewaleInfoDTO
    {
        [JsonProperty("type")]
        public QuestionType Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
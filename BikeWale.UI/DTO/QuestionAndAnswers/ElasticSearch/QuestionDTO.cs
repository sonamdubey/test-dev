using Newtonsoft.Json;

namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    public class QuestionDTO
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }

        [JsonProperty("questionType")]
        public QuestionTypeDTO QuestionType { get; set; }
    }
}
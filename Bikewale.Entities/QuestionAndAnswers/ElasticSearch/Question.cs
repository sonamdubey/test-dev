using Newtonsoft.Json;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{
    /// <summary>
    /// Created by : Snehal Dange on 19th Oct 2018
    /// Desc: Created Question Entity for binding  search results in UI
    /// </summary>
    public class Question
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }

        [JsonProperty("questionType")]
        public QuestionType QuestionType { get; set; }
    }
}

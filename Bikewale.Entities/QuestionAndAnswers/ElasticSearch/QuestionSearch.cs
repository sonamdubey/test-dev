using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{

    /// <summary>
    /// Created by : Snehal Dange on 16th Oct 2018
    /// Desc :     QuestionSearch created for binding Search Results in UI
    /// </summary>
    public class QuestionSearch
    {
        [JsonProperty("question")]
        public Question Question { get; set; }

        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }

        [JsonProperty("answerCount")]
        public uint AnswerCount { get; set; }

        [JsonProperty("answers")]
        public IEnumerable<Answer> Answers { get; set; }

        [JsonProperty("answer")]
        public Answer Answer { get; set; }
    }

}

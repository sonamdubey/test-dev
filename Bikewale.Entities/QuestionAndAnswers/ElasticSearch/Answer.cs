using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{
    /// <summary>
    /// Created by : Snehal Dange on 19th Oct 2018
    /// Desc : Answer entity to store answer from Elastic Index
    /// </summary>
    public class Answer
    {
        [JsonProperty("answerId")]
        public uint AnswerId { get; set; }

        [JsonProperty("answerText")]
        public string AnswerText { get; set; }

        [JsonProperty("answeredBy")]
        public Author AnsweredBy { get; set; }

        [JsonProperty("answeredOn")]
        public DateTime AnsweredOn { get; set; }

        [JsonProperty("answerAge")]
        public string AnswerAge { get; set; }
    }
}

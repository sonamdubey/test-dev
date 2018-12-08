using Bikewale.DTO.Customer;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    public class AnswerDTO
    {
        [JsonProperty("answerId")]
        public uint AnswerId { get; set; }

        [JsonProperty("answerText")]
        public string AnswerText { get; set; }

        [JsonProperty("answeredBy")]
        public AuthorDTO AnsweredBy { get; set; }

        [JsonProperty("answeredOn")]
        public DateTime AnsweredOn { get; set; }

        [JsonProperty("answerAge")]
        public string AnswerAge { get; set; }
    }
}
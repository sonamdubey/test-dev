using Bikewale.DTO.Customer;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.QuestionAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 25 June 2018
    /// Description : DTO for answer entity
    /// </summary>
    public class AnswerDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("strippedText")]
        public string StrippedText { get; set; }
        [JsonProperty("answeredBy")]
        public CustomerBase AnsweredBy { get; set; }
        [JsonProperty("answeredOn")]
        public DateTime AnsweredOn { get; set; }
        [JsonProperty("answerAge")]
        public string AnswerAge { get; set; }
    }
}

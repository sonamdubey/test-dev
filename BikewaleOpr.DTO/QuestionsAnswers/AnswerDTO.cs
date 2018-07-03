using Newtonsoft.Json;

namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 19th June 2018
    /// Desc : AnswerDTO to store answer from opr
    /// Modified by : Sanskar Gupta on 27 June 2018
    /// Description : Added `QuestionText`, `AskedByEmail` and `AskedByName`
    /// </summary>
    public class AnswerDTO : AnswerBaseDTO
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }
        [JsonProperty("questionText")]
        public string QuestionText { get; set; }
        [JsonProperty("askedByEmail")]
        public string AskedByEmail { get; set; }
        [JsonProperty("askedByName")]
        public string AskedByName { get; set; }
    }
}

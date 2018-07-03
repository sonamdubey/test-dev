using BikewaleOpr.DTO.Customer;
using Newtonsoft.Json;

namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 19th June 2018
    /// Desc : CReated to store answer from opr
    /// </summary>
    public class AnswerBaseDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("answeredBy")]
        public CustomerBaseDTO AnsweredBy { get; set; }
    }
}

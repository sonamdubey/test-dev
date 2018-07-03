using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : DTO to hold the list of `Answers`
    /// </summary>
    public class AnswersDTO
    {
        [JsonProperty("answers")]
        public IEnumerable<AnswerDTO> Answers { get; set; }
    }
}

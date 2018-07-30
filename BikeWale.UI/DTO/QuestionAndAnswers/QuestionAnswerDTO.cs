using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.QuestionAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 25 June 2018
    /// Description : DTO to store Question and Answer
    /// </summary>
    public class QuestionAnswerDTO
    {
        [JsonProperty("question")]
        public QuestionDTO Question { get; set; }
        [JsonProperty("answer")]
        public AnswerDTO Answer { get; set; }
    }
}

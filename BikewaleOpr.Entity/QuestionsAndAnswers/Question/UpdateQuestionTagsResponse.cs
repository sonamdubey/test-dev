using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QuestionsAndAnswers.Question
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 14 June 2018
    /// Description : Class to hold response for UpdateQuestionTags API
    /// </summary>
    public class UpdateQuestionTagsResponse
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }
        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }
    }
}

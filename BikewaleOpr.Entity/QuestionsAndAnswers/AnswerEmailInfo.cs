using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QuestionsAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 09 July 2018
    /// Description : Entity to hold information for email sent to the user to answer the question.
    /// </summary>
    public class AnswerEmailInfo
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string QuestionId { get; set; }
        public string Url { get; set; }
    }
}

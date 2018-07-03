using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA.Question
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 13 June 2018
    /// Description : Response for `ModerateQuestion` controller
    /// </summary>
    public class QuestionModerationResponse
    {
        public string questionId { get; set; }
        public bool isModeratedSuccessfully { get; set; }
        public string message { get; set; }
    }
}

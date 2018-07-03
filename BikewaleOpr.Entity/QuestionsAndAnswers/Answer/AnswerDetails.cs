using BikewaleOpr.Entity.QnA.Question;
using BikewaleOpr.Entity.QnA.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA.Answer
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Entire Answer Details for a particular entry in Manage Answers Table
    /// </summary>
    
    public class AnswerDetails
    {
        public QuestionDetails QuestionDetails{ get; set; }
        public Answer Answer{ get; set; }
        public DateTime AnsweredDate { get; set; }
        public InternalUser AnsweredBy { get; set; }
    }
}

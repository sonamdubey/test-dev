using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.QuestionsAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: VM for Manage Answers Table
    /// </summary>
    

    public class ManageAnswersVM: QnABaseVM
    {
        public IEnumerable<AnswerDetails> AnswersList { get; set; } 
    }
}

using BikewaleOpr.Entity.QnA.Question;
using System.Collections.Generic;

namespace BikewaleOpr.Models.QuestionsAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: VM for Manage Questions Table
    /// </summary>


    public class ManageQuestionsVM : QnABaseVM
    {
        public IEnumerable<Question> QuestionsList { get; set; }
        public int TotalRecordCount { get; set; }
        public int CurrentUserId { get; set; }
        public QuestionDetailsBaseVM DetailsPopups { get; set; } 
    }
}

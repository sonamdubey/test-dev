using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.User;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Entire Question Details for a particular entry in Manage Questions Table
    /// </summary>

    public class QuestionDetails
    {
        public BikewaleOpr.Entity.QnA.Question.Question Question { get; set; }
        public EndUser EndUser { get; set; }
        public DateTime EntryDate { get; set; }
        public IEnumerable<BikewaleOpr.Entity.QnA.Tag> Tags { get; set; }
        public EnumQuestionStatus Status { get; set; }
    }
}

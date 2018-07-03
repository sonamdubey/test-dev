
using BikewaleOpr.Entity.QnA.User;
using System;
namespace BikewaleOpr.Entity.QnA.Question
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Contains basic question text
    /// </summary>

    public class Question
    {
        public System.Collections.Generic.IEnumerable<Tag> Tags { get; set; }
        public string TagsCSV { get; set; }
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public DateTime AskedOn { get; set; }
        public EndUser EndUser { get; set; }
        public EnumQuestionStatus Status { get; set; }
        public uint AnswerCount { get; set; }
    }
}

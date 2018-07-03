using Bikewale.Entities.Customer;
using System;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 11 June 2018
    /// Description: Entity to hold information about Answer to a certain question.
    /// </summary>
    public class Answer
    {
        public uint Id { get; set; }
        public string Text { get; set; }
        public CustomerEntityBase AnsweredBy { get; set; }
        public DateTime AnsweredOn { get; set; }
        public string StrippedText { get; set; }
        public string AnswerAge { get; set; }
    }
}

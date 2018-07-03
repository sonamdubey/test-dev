using System;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 12 June 2018
    /// Description: Entity to store details about Answers.
    /// Modified by : Sanskar Gupta on 20 June 2018
    /// Description : Added property `AnsweredOn`
    /// </summary>
    [Serializable]
    public class AnswerBase
    {
        public uint Id { get; set; }
        public string Text { get; set; }
        public Customer AnsweredBy { get; set; }
        public DateTime AnsweredOn { get; set; }
    }
}

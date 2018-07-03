using System;
using System.Collections.Generic;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : Entity to hold the properties returned by the SP
    /// Modified By : Deepak Israni on 12 June 2018
    /// Description : Modified entity to mirror schema.
    /// Modified by : Sanskar Gupta on 19 June 2018
    /// Description : Added `Answers` property.
    /// </summary>
    [Serializable]
    public class Question
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public DateTime AskedOn { get; set; }
        public Customer AskedBy { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public EnumModerationStatus Status { get; set; }
        public IEnumerable<AnswerBase> Answers { get; set; }
        public uint AnswerCount { get; set; }
    }
}

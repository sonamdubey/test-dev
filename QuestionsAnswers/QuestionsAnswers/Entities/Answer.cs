
using System;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Modified by : Sanskar Gupta on 27 June 2018
    /// Description : Added `QuestionText`, `AskedByEmail` and `AskedByName`
    /// </summary>
    [Serializable]
    public class Answer : AnswerBase
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AskedByEmail { get; set; }
        public string AskedByName { get; set; }
    }
}

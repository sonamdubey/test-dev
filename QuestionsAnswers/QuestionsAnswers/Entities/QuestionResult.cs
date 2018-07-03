using System;
using System.Collections.Generic;

namespace QuestionsAnswers.Entities
{
    [Serializable]
    public class QuestionResult
    {
        public IEnumerable<Question> QuestionList { get; set; }
        public int TotalRecordCount { get; set; }
    }
}

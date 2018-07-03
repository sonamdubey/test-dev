
using System.Collections.Generic;
namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// stores question and answer list with total question count
    /// </summary>
    public class QuestionAnswerWrapper
    {
        public IEnumerable<QuestionAnswer> QuestionList { get; set; }
        public uint TotalAnsweredQuestions { get; set; }
    }
}

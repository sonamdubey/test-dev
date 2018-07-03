
using QuestionsAnswers.Entities;
using System.Collections.Generic;
namespace QuestionsAnswers.Cache
{
    public interface IQuestionsCacheRepository
    {
        IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds);
    }
}

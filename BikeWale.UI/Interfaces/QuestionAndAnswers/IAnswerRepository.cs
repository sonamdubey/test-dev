
namespace Bikewale.Interfaces.QuestionAndAnswers
{
    public interface IAnswerRepository
    {
        bool CheckDuplicateAnswerByUser(string questionId, uint customerId);
    }

}

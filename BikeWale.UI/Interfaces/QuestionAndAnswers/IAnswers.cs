
using Bikewale.Entities.QuestionAndAnswers;
namespace Bikewale.Interfaces.QuestionAndAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 11th July 2018
    /// Desc : Interface created to handle answer flow through bikewale site
    /// Modified by :   Sumit Kate on 03 Sep 2018
    /// Description :   Modified the SubmitUserAnswer method signature to make it more generic
    /// </summary>
    public interface IAnswers
    {
        bool CheckDuplicateAnswerByUser(string questionId, uint customerId);
        bool SubmitUserAnswer(string questionId,string answerText,string userName,string userEmail, BWClientInfo clientInfo);        
    }
}

using BikewaleOpr.Entity.Users;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : Interface for OPR Cache of QuestionsAndAnswers
    /// </summary>
    public interface IQuestionsAndAnswersCache
    {
        IEnumerable<User> GetInternalUsers();

        HashSet<uint> GetInternalUserIDs();
    }
}

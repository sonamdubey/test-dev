using BikewaleOpr.Entity.Users;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : Interface for `UsersRepository` for QuestionsAnswers
    /// </summary>
    public interface IUsersRepository
    {
        IEnumerable<User> GetInternalUsers();
    }
}

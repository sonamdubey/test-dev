using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : Interface for Answers BAL
    /// </summary>
    public interface IAnswersBAL
    {
        IEnumerable<Answer> GetAnswers(string questionId);
    }
}

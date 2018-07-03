using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.DAL
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : Interface to hold all the methods for `AnswersRepository`
    /// </summary>
    public interface IAnswersRepository
    {
        IEnumerable<Answer> GetAnswers(string questionId);
    }
}

using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.BAL
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// Description : Interface to hold methods for `Answers`
    /// </summary>
    public interface IAnswers
    {
        IEnumerable<Answer> GetAnswers(string questionId);
    }
}

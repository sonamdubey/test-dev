using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QuestionsAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 20 June 2018
    /// Description : Entity to hold list of answers.
    /// </summary>
    
    [Serializable]
    public class AnswersEntity
    {
        public IEnumerable<Answer> Answers { get; set; }
    }
}

using Bikewale.Entities.QuestionAndAnswers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Sep 2018
    /// Description :   View Model for Unanswered Question
    /// </summary>
    public class UnansweredQuestionVM
    {
        public QuestionBase Question { get; set; }
        public Bikewale.Entities.QuestionAndAnswers.Sources SourceId { get; set; }
        public ushort Platform { get; set; }
    }
}
